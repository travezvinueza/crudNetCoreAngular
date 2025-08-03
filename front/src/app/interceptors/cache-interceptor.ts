import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { catchError, of, tap, throwError } from 'rxjs';
import { cache } from '../shared/http-cache';

const TTL = 10 * 60 * 1000; // 10 minutes tiempo de vida de la caché

export const cacheInterceptor: HttpInterceptorFn = (req, next) => {

  if (req.method !== 'GET') {
    return next(req);
  }

  const cacheKey = JSON.stringify({
    url: req.urlWithParams,
    headers: req.headers.keys().reduce((acc, key) => {
      acc[key] = req.headers.get(key);
      return acc;
    }, {} as Record<string, string | null>),
    body: req.body
  });

  const cachedResponse = cache.get(cacheKey);
  if (cachedResponse && cachedResponse.expiresAt > Date.now()) {
    return of(cachedResponse.resp);
  } else if (cachedResponse) {
    cache.delete(cacheKey);
  }

  return next(req).pipe(
    tap((event) => {
      if (event instanceof HttpResponse) {
        cache.set(cacheKey, {
          expiresAt: Date.now() + TTL,
          resp: event
        });
      }
    }),
    catchError((error) => {
      console.error('Cache Interceptor Error:', error);
      return throwError(() => error);
    })
  );

};
