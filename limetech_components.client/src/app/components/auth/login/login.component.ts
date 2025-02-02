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
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }

  onLogin(): void {


    console.log('Login button clicked');


    if (this.loginForm.valid) {
      const credentials = this.loginForm.value;


      console.log('Sending login request with:', credentials);


      this.authService.login(credentials).subscribe({
        next: (response) => {
          console.log('Login successful', response);
          // Redirect to the home page after login
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error('Login failed', err);
          this.loginError = 'Invalid username or password. Please try again.';
        },
      });
    }
    else
    {

      console.log('Form is not valid:', this.loginForm.value);

      console.log('Form status:', this.loginForm.status);
      console.log('Form value:', this.loginForm.value);
      console.log('Username control:', this.loginForm.controls['username'].value);
      console.log('Password control:', this.loginForm.controls['password'].value);


      this.loginError = 'Please fill out all fields.';
    }
  }
}
