import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { BasketService } from '../../services/basket.service';
import { HomeService } from '../../services/home.service';
import { ComponentDTO } from '../../models/component.dto';
import { SearchService } from '../../services/search.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { NavigationEnd } from '@angular/router';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  components: ComponentDTO[] = [];
  topPurchased: ComponentDTO[] = [];
  customerId: string = '';
  categories: string[] = []; // All available types
  producers: string[] = [];  // All available producers

  filteredProducers: string[] = []; // Updated based on selection
  filteredCategories: string[] = []; // Updated based on selection

  filterForm: FormGroup;

  searchKeyword = '';
  showNotification = false;
  defaultFilterApplied = true; // Track if the default filter is applied

  constructor(
    private fb: FormBuilder,
    private homeService: HomeService,
    private basketService: BasketService,
    private searchService: SearchService,
    private authService: AuthService,
    private router: Router
  ) {
    this.filterForm = this.fb.group({
      name: [''],
      producer: [''],
      typeOfProduct: [''],
      minPrice: [null],
      maxPrice: [null],
      productionYear: [null],
      status: [null],
      sortBy: ['']
      //currentPage: 1,
      //componentsPerPage: 10,
    })
  }

  ngOnInit(): void {
    this.loadFilters(); // All data must be available first

    this.customerId = this.authService.getCustomerId() || '';


    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd && event.urlAfterRedirects === '/') {
        // Reset filters and show top purchased when Home is clicked, even if already on home
        this.filterForm.reset();
        this.searchService.updateSearchKeyword(''); // Clear the global search bar
        this.defaultFilterApplied = true;
        this.loadTopPurchased();
        this.components = [];

        window.scrollTo({ top: 0, behavior: 'smooth' }); // Smooth scroll to top
      }
    });


    if (this.isFilterEmpty()) {
      this.loadTopPurchased();
    } else {
      this.loadComponents();
    }

    this.filterForm.valueChanges.subscribe(() => {
      this.loadComponents();
      this.updateAvailableFilters(); // This way I update filters every time a selection changes
    });

    this.searchService.searchKeyword$.subscribe((keyword) => {
      this.searchKeyword = keyword;
      this.filterComponents();
    });
  }

  filterComponents(): void {
    console.log('Filtering components with:', this.searchKeyword);

    if (!this.searchKeyword.trim()) {
      return; // Don't trigger search if empty
    }

    const searchQuery = this.searchKeyword.trim();

    this.filterForm.patchValue({
      name: searchQuery,
      producer: '', // Clear producer when doing a keyword search
      typeOfProduct: '', // Clear typeOfProduct when doing a keyword search
      productionYear: null, // Ensure productionYear is null instead of string
    });

    this.defaultFilterApplied = false;
    this.loadComponents();
  }




  loadComponents(): void {
    const isFilterActive = !this.isFilterEmpty();
    this.defaultFilterApplied = !isFilterActive;

    if (isFilterActive) {
      this.homeService.getComponents(this.filterForm.value).subscribe({
        next: (data: any) => {
          this.components = data.components;
          this.updateAvailableFilters();
        },
        error: (error) => console.error('Failed to load components', error),
      });
    } else {
      // Ensure this API call exists in your backend
      this.homeService.getComponentsSortedByYear().subscribe({
        next: (data) => {
          this.components = data;
          this.updateAvailableFilters();
        },
        error: (error) => console.error('Failed to load all components', error),
      });
    }
  }




  loadTopPurchased(): void {
    this.homeService.getTopComponents().subscribe({
      next: (data) => this.topPurchased = data,
      error: (error) => console.error('Failed to load top purchased components', error),
    });
  }


  updateAvailableFilters(): void {
    const selectedProducer = this.filterForm.value.producer;
    const selectedType = this.filterForm.value.typeOfProduct;

    if (selectedProducer && !selectedType) {
      // If a producer is selected, filter categories based on the full dataset
      this.filteredCategories = [...new Set(
        this.categories.filter(category =>
          this.components.some(c => c.producer === selectedProducer && c.typeOfProduct === category)
        )
      )];
    }
    else if (!selectedProducer && selectedType) {
      // If a category (type) is selected, filter producers based on the full dataset
      this.filteredProducers = [...new Set(
        this.producers.filter(producer =>
          this.components.some(c => c.typeOfProduct === selectedType && c.producer === producer)
        )
      )];
    }
    else if (selectedProducer && selectedType) {
      // If both are selected, ensure both lists are filtered correctly
      this.filteredProducers = [...new Set(
        this.producers.filter(producer =>
          this.components.some(c => c.producer === producer && c.typeOfProduct === selectedType)
        )
      )];
      this.filteredCategories = [...new Set(
        this.categories.filter(category =>
          this.components.some(c => c.typeOfProduct === category && c.producer === selectedProducer)
        )
      )];
    }
    else {
      // If nothing is selected, show all available options
      this.filteredProducers = [...this.producers];
      this.filteredCategories = [...this.categories];
    }
  }

  loadFilters(): void {
    this.homeService.getAllComponents().subscribe({
      next: (data) => {
        this.categories = [...new Set(data.map(c => c.typeOfProduct).filter(x => x !== null))] as string[];
        this.producers = [...new Set(data.map(c => c.producer).filter(x => x !== null))] as string[];

        // Initially, all options are available
        this.filteredCategories = [...this.categories];
        this.filteredProducers = [...this.producers];
      },
      error: (error) => console.error('Failed to load filters', error),
    });
  }


  addToBasket(componentId: number): void {

    this.customerId = this.authService.getCustomerId() || '';

    if (!this.authService.isAuthenticated()) {
      console.warn("User is not logged in. Redirecting to login page...");
      this.router.navigate(['/login']);
      return;
    }

    this.basketService.addToBasket(componentId).subscribe({
      next: () => {
        this.showNotification = true;
        setTimeout(() => (this.showNotification = false), 3000);
      },
      error: (error) => console.error("Error adding to basket:", error),
    });
  }



  private isFilterEmpty(): boolean {
    const values = this.filterForm.value;
    return !values.name &&
      !values.producer &&
      !values.typeOfProduct &&
      !values.minPrice &&
      !values.maxPrice &&
      !values.productionYear &&
      !values.status &&
      !values.sortBy;
  }


  sortBy(property: keyof ComponentDTO) {
    this.components.sort((a, b) => {
      if (typeof a[property] === 'string') {
        return (a[property] as string).localeCompare(b[property] as string);
      }
      return (a[property] as number) - (b[property] as number);
    });
  }

}
