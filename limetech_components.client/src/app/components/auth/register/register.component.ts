import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, switchMap, catchError, of } from 'rxjs';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerForm: FormGroup;
  usernameMessage: string = '';
  emailMessage: string = '';


  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router) {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });

    // Username Validation
    this.registerForm.get('username')?.valueChanges
      .pipe(
        debounceTime(300),
        switchMap(username => this.authService.checkUsername(username)),
        catchError(() => of({ message: "Username validation failed." }))
      )
      .subscribe({
        next: () => this.usernameMessage = '',
        error: (err) => this.usernameMessage = err.error?.message || 'Error checking username.'
      });

    // Email Validation
    this.registerForm.get('email')?.valueChanges
      .pipe(
        debounceTime(300),
        switchMap(email => this.authService.checkEmail(email)),
        catchError(() => of({ message: "Email validation failed." }))
      )
      .subscribe({
        next: () => this.emailMessage = '',
        error: (err) => this.emailMessage = err.error?.message || 'Error checking email.'
      });
  }

  onRegister(): void {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe({
        next: (response) => {
          console.log('Registration successful', response);
          this.router.navigate(['/login']);
        },
        error: (err) => {
          console.error('Registration failed', err);
        },
      });
    }
    else {
      console.error('Form is invalid');
    }
  }
}
