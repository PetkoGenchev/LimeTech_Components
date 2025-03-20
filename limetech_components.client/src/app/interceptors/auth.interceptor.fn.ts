import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpEvent } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { NgZone } from '@angular/core';
import { catchError, switchMap, throwError, from, Observable } from 'rxjs';

let isRefreshing = false;
let refreshTokenSubject: Promise<string | null> | undefined;

export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const ngZone = inject(NgZone);

  const accessToken = authService.getLatestAccessToken();

  const clonedRequest = accessToken
    ? req.clone({ setHeaders: { Authorization: `Bearer ${accessToken}` } })
    : req;

  return next(clonedRequest).pipe(
    catchError(error => {
      if (error.status === 401) {
        console.warn('Access token expired, attempting to refresh token...');

        if (!isRefreshing) {
          isRefreshing = true;
          refreshTokenSubject = authService.refreshToken().toPromise()
            .then(response => {
              isRefreshing = false;
              refreshTokenSubject = undefined; // Reset after completion

              if (!response || !response.accessToken) {
                console.error("No access token received, logging out...");
                authService.logout();
                ngZone.run(() => router.navigate(['/login']));
                return null;
              }

              authService.updateTokens(response);
              return response.accessToken;
            })
            .catch(refreshError => {
              isRefreshing = false;
              refreshTokenSubject = undefined;
              console.error('Refresh token failed:', refreshError);
              authService.logout();
              ngZone.run(() => router.navigate(['/login']));
              return null;
            });
        }

        return from(refreshTokenSubject ?? Promise.resolve(null)).pipe(
          switchMap(newToken => {
            if (!newToken) return throwError(() => error);
            const newRequest = req.clone({
              setHeaders: { Authorization: `Bearer ${newToken}` }
            });
            return next(newRequest);
          })
        );
      }

      return throwError(() => error);
    })
  );
};
