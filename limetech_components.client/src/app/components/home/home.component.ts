import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { BasketService } from '../../services/basket.service';
import { ComponentService } from '../../services/component.service';
import { ComponentDTO } from '../../models/component.dto';
import { SearchService } from '../../services/search.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';


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
    private componentService: ComponentService,
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


    // Listen for route changes to detect when Home is clicked
    this.router.events.subscribe(() => {
      if (this.router.url === '/home') {
        this.loadTopPurchased();
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

    // Prepare the search query
    const searchQuery = this.searchKeyword.trim();

    // Set the filter value for backend query
    this.filterForm.patchValue({
      name: searchQuery,
      producer: searchQuery,
      typeOfProduct: searchQuery,
      productionYear: searchQuery
    });

    this.defaultFilterApplied = false; // Show search results
    this.loadComponents();
  }




  loadComponents(): void {
    const isFilterActive = !this.isFilterEmpty(); // Check if any filter is applied
    this.defaultFilterApplied = !isFilterActive; // Show Best Sellers only if no filters are used

    if (isFilterActive) {
      this.componentService.getComponents(this.filterForm.value).subscribe({
        next: (data: any) => {
          this.components = data.components;
          this.updateAvailableFilters();
        },
        error: (error) => console.error('Failed to load components', error),
      });
    } else {
      this.componentService.getComponentsSortedByYear().subscribe({
        next: (data) => {
          this.components = data;
          this.updateAvailableFilters();
        },
        error: (error) => console.error('Failed to load all components', error),
      });
    }
  }



  loadTopPurchased(): void {
    this.componentService.getTopComponents().subscribe({
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
    this.componentService.getAllComponents().subscribe({
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
