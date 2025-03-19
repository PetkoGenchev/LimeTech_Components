import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { catchError, switchMap, tap } from 'rxjs/operators';

interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  userId: string;
  role: string;
  customerId?: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  private apiUrl = 'https://localhost:7039/api/auth';

  private authStatusSubject = new BehaviorSubject<{ isSignedIn: boolean; isAdmin: boolean }>({
    isSignedIn: !!this.getAccessToken(),
    isAdmin: this.getUserRole() === 'Admin',
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


  login(credentials: { username: string; password: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => this.handleAuthResponse(response)),
      catchError(error => {
        console.error('Login failed', error);
        return throwError(() => error);
      })
    );
  }


  refreshToken(): Observable<AuthResponse> {
    const refreshToken = this.getRefreshToken();
    const accessToken = this.getAccessToken();

    if (!refreshToken || !accessToken) {
      return throwError(() => new Error('No refresh token available'));
    }

    return this.http.post<AuthResponse>(`${this.apiUrl}/refresh`, { accessToken, refreshToken }).pipe(
      tap(response => {
        console.log("Refresh Token Response:", response);
        this.updateTokens(response);
      }),
      catchError(error => {
        console.error('Refresh token failed', error);
        this.logout();
        return throwError(() => error);
      })
    );
  }

  getCustomerId(): string | null {
    return localStorage.getItem('customerId');
  }


  logout(): Observable<void> {
    this.clearTokens();
    this.authStatusSubject.next({ isSignedIn: false, isAdmin: false });
    return of(void 0); // Return an observable so .subscribe() works
  }


  isAuthenticated(): boolean {
    return !!this.getAccessToken();
  }


  getUserRole(): string | null {
    return localStorage.getItem('role');
  }



  validateSession(): Observable<boolean> {
    const accessToken = this.getAccessToken();
    if (!accessToken) {
      this.logout();
      return of(false);
    }

    return this.http.get<boolean>(`${this.apiUrl}/validate-session`, {
      headers: { Authorization: `Bearer ${accessToken}` }
    }).pipe(
      catchError(error => {
        if (error.status === 401) {
          console.warn("Session expired, attempting token refresh...");

          return this.refreshToken().pipe(
            switchMap(() => {
              const newAccessToken = this.getAccessToken(); // Fetch latest token AFTER refresh
              console.log("Using new access token for validation:", newAccessToken);

              if (!newAccessToken) {
                console.error("Token refresh failed, logging out.");
                this.logout();
                return of(false);
              }

              return this.http.get<boolean>(`${this.apiUrl}/validate-session`, {
                headers: { Authorization: `Bearer ${newAccessToken}` }
              });
            }),
            catchError(() => {
              console.error("Session validation failed after refresh.");
              this.logout();
              return of(false);
            })
          );
        }

        console.error("Unexpected error in session validation:", error);
        this.logout();
        return of(false);
      })
    );
  }




  private handleAuthResponse(response: AuthResponse): void {
    this.storeTokens(response);
    this.authStatusSubject.next({ isSignedIn: true, isAdmin: response.role === 'Admin' });
  }

  private storeTokens(response: AuthResponse): void {
    console.log("Storing Tokens:", response);
    localStorage.setItem('accessToken', response.accessToken);
    localStorage.setItem('refreshToken', response.refreshToken);
    localStorage.setItem('userId', response.userId);
    localStorage.setItem('role', response.role);

    if (response.customerId) {
      localStorage.setItem('customerId', response.customerId);
    } else {
      console.warn("Customer ID is missing in login response!");
      localStorage.removeItem('customerId');
    }
  }

  private clearTokens(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('userId');
    localStorage.removeItem('role');
    localStorage.removeItem('customerId');
  }

  private getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  private getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  private getAuthHeaders(): HttpHeaders {
    return new HttpHeaders({
      Authorization: `Bearer ${this.getAccessToken()}`,
    });
  }

  getLatestAccessToken(): string | null {
    return this.getAccessToken();
  }

  updateTokens(response: AuthResponse): void {
    this.storeTokens(response);
  }

}
