import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'LimeTech Components';

  isSignedIn = false;
  isAdmin = false;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.authService.authStatus$.subscribe((authStatus) => {
      this.isSignedIn = authStatus.isSignedIn;
      this.isAdmin = authStatus.isAdmin;
    });
  }

  logout(): void {
    this.authService.logout().subscribe({
      next: () => {
        console.log('Logged out successfully');
        this.isSignedIn = false;
        this.isAdmin = false;
        window.location.href = '/';
      },
      error: (err) => {
        console.error('Logout failed', err);
      },
    });
  }

}
