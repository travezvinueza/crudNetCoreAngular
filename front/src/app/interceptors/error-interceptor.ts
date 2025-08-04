import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { toast } from 'ngx-sonner';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 0) {
        toast.error('Sin conexión con el servidor');
      } else if (error.error?.message) {
        toast.error(error.error.message);
      } else {
        toast.error('Ocurrió un error inesperado');
      }

      return throwError(() => error); 
    })
  );
};
