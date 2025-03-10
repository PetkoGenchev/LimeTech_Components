import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

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

        const isAdmin = response.role === 'Admin';

        localStorage.setItem('userId', response.userId);
        if (response.customerId) {
          localStorage.setItem('customerId', response.customerId);
        }

        this.authStatusSubject.next({ isSignedIn: true, isAdmin });
      }),
      catchError((error) => {
        console.error('Login failed', error);
        return throwError(() => error)
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
        this.authStatusSubject.next({ isSignedIn: false, isAdmin: false });
      })
    );
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('customerId') || !!localStorage.getItem('userId');
  }

}
