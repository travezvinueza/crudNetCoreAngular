import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { catchError, of, tap, throwError } from 'rxjs';

const cache = new Map<string, { expiresAt: number, resp: HttpResponse<unknown> }>();
const TTL = 20 * 60 * 1000; // 20 minutes tiempo de vida de la caché

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
  });

  const cachedResponse = cache.get(cacheKey);
  if (cachedResponse && cachedResponse.expiresAt > Date.now()) {
    return of(cachedResponse.resp.clone());
  } else if (cachedResponse) {
    cache.delete(cacheKey);
  }

  return next(req).pipe(
    tap((event) => {
      if (event instanceof HttpResponse) {
        cache.set(cacheKey, {
          expiresAt: Date.now() + TTL,
          resp: event.clone()
        });
      }
    }),
    catchError((error) => {
      console.error('Cache Interceptor Error:', error);
      return throwError(() => error);
    })
  );

};
