import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;
  loginError: string = '';

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }





  //onLogin(): void {
  //  const credentials = {
  //    username: 'LimeUsername',
  //    password: 'lime123',
  //  };

  //  this.authService.login(credentials).subscribe({
  //    next: (response) => {
  //      console.log('Login successful', response);
  //    },
  //    error: (err) => {
  //      console.error('Login failed', err);
  //    },
  //  });
  //}
}
