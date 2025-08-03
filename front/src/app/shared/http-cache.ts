import { HttpResponse } from '@angular/common/http';

export const cache = new Map<string, { expiresAt: number, resp: HttpResponse<unknown> }>();

export function invalidateCacheByUrlFragment(fragment: string) {
  if (cache.size === 0) {
    return;
  }
  for (const key of cache.keys()) {
    if (key.includes(fragment)) {
      cache.delete(key);
    }
  }
}
