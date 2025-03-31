import { Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service';
import { RouterModule } from '@angular/router';
import { SearchService } from './services/search.service';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'LimeTech Components';
  isSignedIn = false;
  isAdmin = false;
  searchKeyword: string = '';
  private searchTimeout: any;

  constructor(private authService: AuthService, private searchService: SearchService) { }

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
      error: (err: any) => {
        console.error('Logout failed', err);
      },
    });
  }

  onSearchInput(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    if (!inputElement) return;

    let value = inputElement.value.trim();

    // Max 50 characters
    if (value.length > 50) {
      inputElement.value = value.slice(0, 50);
      inputElement.value = value; // Update input value
    }

    clearTimeout(this.searchTimeout);

    this.searchTimeout = setTimeout(() => {
      this.searchService.updateSearchKeyword(value);
    }, 2000); // 2 seconds delay
  }

  onKeyPress(event: KeyboardEvent): void {
    if (event.key === ' ' || event.key === 'Enter') {
      clearTimeout(this.searchTimeout);
      this.searchService.updateSearchKeyword((event.target as HTMLInputElement).value.trim());
    }
  }

}
