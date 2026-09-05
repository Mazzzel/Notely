import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const authErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const isAuthProbe = req.url.includes('/Auth/Login') || req.url.includes('/Auth/Me');
      if (error.status === 401 && !isAuthProbe && !router.url.startsWith('/login')) {
        router.navigate(['/login']);
      }
      return throwError(() => error);
    })
  );
};
