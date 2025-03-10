import { Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service';
import { RouterModule } from '@angular/router';
/*import { HomeComponent } from './components/home/home.component';*/
import { SearchService } from './services/search.service';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule /*HomeComponent*/],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'LimeTech Components';

  isSignedIn = false;
  isAdmin = false;
  searchKeyword: string = '';

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
      error: (err) => {
        console.error('Logout failed', err);
      },
    });
  }

  onSearchInput(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement) {
      this.searchService.updateSearchKeyword(inputElement.value);
    }
  }

}
