import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  constructor(private authService: AuthService) { }

  onRegister(): void {
    const user = {
      username: 'LimeUsername',
      email: 'lime@lime.com',
      password: 'lime123',
    };

    this.authService.register(user).subscribe({
      next: (response) => {
        console.log('Registration successful', response);
      },
      error: (err) => {
        console.error('Registration failed', err);
      },
    });
  }
}
