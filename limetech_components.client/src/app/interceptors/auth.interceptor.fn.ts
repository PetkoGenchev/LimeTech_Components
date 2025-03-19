import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  let accessToken = authService.getLatestAccessToken();

  if (accessToken) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${accessToken}` }
    });
  }

  return next(req).pipe(
    catchError(error => {
      if (error.status === 401) {
        console.warn('Access token expired, attempting to refresh token...');

        return authService.refreshToken().pipe(
          switchMap((response) => {
            // Directly use the new access token from the response
            const newAccessToken = response.accessToken;
            if (newAccessToken) {
              authService.updateTokens(response); // Ensure tokens are stored properly
              req = req.clone({
                setHeaders: { Authorization: `Bearer ${newAccessToken}` }
              });
            }
            return next(req); // Retry the request with the new token
          }),
          catchError(refreshError => {
            console.error('Refresh token failed:', refreshError);
            authService.logout(); // Clear session properly
            router.navigate(['/login']);
            return throwError(() => refreshError);
          })
        );
      }
      return throwError(() => error);
    })
  );
};
