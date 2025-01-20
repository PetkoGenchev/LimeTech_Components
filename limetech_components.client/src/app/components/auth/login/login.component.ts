import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  constructor(private authService: AuthService) { }

  onLogin(): void {
    const credentials = {
      username: 'LimeUsername',
      password: 'lime123',
    };

    this.authService.login(credentials).subscribe({
      next: (response) => {
        console.log('Login successful', response);
      },
      error: (err) => {
        console.error('Login failed', err);
      },
    });
  }
}
