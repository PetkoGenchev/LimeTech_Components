import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { catchError, switchMap, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  private apiUrl = 'https://localhost:7039/api/auth';

  private authStatusSubject = new BehaviorSubject<{ isSignedIn: boolean; isAdmin: boolean }>({
    isSignedIn: false,
    isAdmin: false,
  });

  authStatus$ = this.authStatusSubject.asObservable();

  constructor(private http: HttpClient) { }

  register(user: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  checkUsername(username: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/check-username`, {
      params: { username },
    });
  }

  checkEmail(email: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/check-email`, {
      params: { email },
    });
  }


  login(credentials: { username: string, password: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
      tap((response) => {

        console.log("Login successful!", response);

        localStorage.setItem('accessToken', response.accessToken);
        localStorage.setItem('refreshToken', response.refreshToken);
        localStorage.setItem('userId', response.userId);
        localStorage.setItem('role', response.role);

        if (response.customerId) {
          localStorage.setItem('customerId', response.customerId);
        }

        const isAdmin = response.role === 'Admin';

        this.authStatusSubject.next({ isSignedIn: true, isAdmin });
      }),
      catchError((error) => {
        console.error('Login failed', error);
        return throwError(() => error)
      })
    );
  }


  refreshToken(): Observable<any> {
    const refreshToken = localStorage.getItem('refreshToken');
    const accessToken = localStorage.getItem('accessToken');

    if (!refreshToken || !accessToken) {
      return throwError(() => new Error('No refresh token available'));
    }

    return this.http.post<any>(`${this.apiUrl}/auth/refresh`, { accessToken, refreshToken }).pipe(
      tap(response => {
        localStorage.setItem('accessToken', response.accessToken);
        localStorage.setItem('refreshToken', response.refreshToken);
      }),
      catchError(error => {
        console.error('Refresh token failed', error);
        this.logout(); // Log out if refresh fails
        return throwError(() => error);
      })
    );
  }

  getCustomerId(): string | null {
    return localStorage.getItem('customerId');
  }

  logout(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/logout`, {}).pipe(
      tap(() => {
        localStorage.removeItem('customerId');
        localStorage.removeItem('userId');
        localStorage.removeItem('accessToken');
        localStorage.removeItem('role');
        this.authStatusSubject.next({ isSignedIn: false, isAdmin: false });
      })
    );
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('customerId') && !!localStorage.getItem('userId');
  }


  validateSession(): Observable<boolean> {
    const accessToken = localStorage.getItem('accessToken');
    if (!accessToken) {
      this.logout();
      return of(false);
    }

    return this.http.get<boolean>(`${this.apiUrl}/validate-session`, {
      headers: { Authorization: `Bearer ${accessToken}` }
    }).pipe(
      catchError(error => {
        if (error.status === 401) {
          // If Unauthorized (token expired), try refreshing the token
          return this.refreshToken().pipe(
            switchMap(() => this.validateSession()), // Retry session validation
            catchError(() => {
              this.logout();
              return of(false);
            })
          );
        }
        this.logout();
        return of(false);
      })
    );
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('accessToken');
    return !!token; // Returns true if token exists, false otherwise
  }

  getUserRole(): string | null {
    return localStorage.getItem('role'); // 'Admin' or 'Customer'
  }



}
