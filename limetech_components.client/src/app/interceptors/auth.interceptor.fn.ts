import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const accessToken = localStorage.getItem('accessToken');

  if (accessToken) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  return next(req).pipe(
    catchError((error) => {
      if (error.status === 401) {
        console.warn('Access token expired, attempting to refresh token...');

        return authService.refreshToken().pipe(
          switchMap(() => {
            const newAccessToken = localStorage.getItem('accessToken');
            if (newAccessToken) {
              req = req.clone({
                setHeaders: { Authorization: `Bearer ${newAccessToken}` }
              });
            }
            return next(req);
          }),
          catchError(refreshError => {
            console.error('Refresh token failed:', refreshError);
            authService.logout();
            router.navigate(['/login']);
            return throwError(() => refreshError);
          })
        );
      }
      return throwError(() => error);
    })
  );
};
