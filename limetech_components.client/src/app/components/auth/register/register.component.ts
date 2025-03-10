import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
/*import { debounceTime, switchMap, catchError, of } from 'rxjs';*/
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
  passwordMessage: string = '';
  generalErrorMessage: string = '';


  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router) {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.pattern(/^(?=.*\d).{6,}$/),
        ],
      ],
      fullName: ['', [Validators.required, Validators.minLength(2)]],
    });
  }

  // Check username only when user leaves input field
  checkUsername() {
    const username = this.registerForm.get('username')?.value;
    if (username && username.length >= 2) {
      this.authService.checkUsername(username).subscribe({
        next: () => this.usernameMessage = '',
        error: (err) => this.usernameMessage = err.error?.message || 'Error checking username.'
      });
    }
  }

  // Check email only when user leaves input field
  checkEmail() {

    const emailControl = this.registerForm.get('email');
    if (emailControl?.invalid && emailControl?.touched) {
      this.emailMessage = 'Please enter a valid email.';
      return;
    }

    const email = this.registerForm.get('email')?.value;
    if (email && this.registerForm.get('email')?.valid) {
      this.authService.checkEmail(email).subscribe({
        next: () => this.emailMessage = '',
        error: (err) => this.emailMessage = err.error?.message || 'Error checking email.'
      });
    }
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
          if (err.error && typeof err.error === 'object') {
            this.handleValidationErrors(err.error);
          } else {
            this.generalErrorMessage = 'An error occurred. Please try again.';
          }
        },
      });
    } else {
      console.error('Form is invalid');
    }
  }

  handleValidationErrors(errors: any) {
    this.passwordMessage = '';
    this.usernameMessage = '';
    this.emailMessage = '';

    if (errors['']) {
      errors[''].forEach((msg: string) => {
        if (msg.toLowerCase().includes('password')) {
          this.passwordMessage = msg;
        }
      });
    }
  }
}
