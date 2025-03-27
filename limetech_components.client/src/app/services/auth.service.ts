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

  //private getAuthHeaders(): HttpHeaders {
  //  return new HttpHeaders({
  //    Authorization: `Bearer ${this.getAccessToken()}`,
  //  });
  //}

  getLatestAccessToken(): string | null {
    return this.getAccessToken();
  }

  updateTokens(response: AuthResponse): void {
    this.storeTokens(response);
  }

}
