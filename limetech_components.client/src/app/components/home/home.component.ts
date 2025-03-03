import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { BasketService } from '../../services/basket.service';
import { ComponentService } from '../../services/component.service';
import { ComponentDTO } from '../../models/component.dto';
import { SearchService } from '../../services/search.service';

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
  categories: string[] = []; // All available types
  producers: string[] = [];  // All available producers

  filteredProducers: string[] = []; // Updated based on selection
  filteredCategories: string[] = []; // Updated based on selection

  filterForm: FormGroup;

  searchKeyword = '';

  constructor(
    private fb: FormBuilder,
    private componentService: ComponentService,
    private basketService: BasketService,
    private searchService: SearchService,
  ) {
    this.filterForm = this.fb.group({
      name: [''],
      producer: [''],
      typeOfProduct: [''],
      minPrice: [null],
      maxPrice: [null],
      productionYear: [null],
      status: [null],
      sortBy:['']
      //currentPage: 1,
      //componentsPerPage: 10,
    })
  }

  ngOnInit(): void {
    this.loadFilters();

    this.filterForm.valueChanges.subscribe(() => {
      this.loadComponents();
      this.updateAvailableFilters(); // Filters will be updated on every new selection
    });

    if (this.isFilterEmpty()) {
      this.loadTopPurchased();
    } else {
      this.loadComponents();
    }

    this.searchService.searchKeyword$.subscribe((keyword) => {
      this.searchKeyword = keyword;
      this.filterComponents();
    });
  }


  filterComponents(): void {
    console.log('Filtering components with:', this.searchKeyword);
  }


  //loadComponents(): void {
  //  this.componentService.getComponents(this.filterForm.value).subscribe({
  //    next: (data) => this.components = data,
  //    error: (error) => console.error('Failed to load components', error),
  //  });
  //}


  //loadComponents(): void {
  //  this.componentService.getComponents(this.filterForm.value).subscribe({
  //    next: (data: any) => { // I can also create a new interface for paginated response instead of "any"
  //      console.log('API Response:', data);
  //      this.components = data.components;
  //    },
  //    error: (error) => console.error('Failed to load components', error),
  //  });
  //}

  loadComponents(): void {
    this.componentService.getComponents(this.filterForm.value).subscribe({
      next: (data: any) => {
        console.log('API Response:', data);
        this.components = data.components;
      },
      error: (error) => console.error('Failed to load components', error),
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



  loadTopPurchased(): void {
    this.componentService.getTopComponents().subscribe({
      next: (data) => this.topPurchased = data,
      error: (error) => console.error('Failed to load top purchased components', error),
    });
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
    this.basketService.addToBasket(componentId).subscribe(() => {
      console.log(`Component ${componentId} added to basket.`);
    });
  }

  private isFilterEmpty(): boolean {
    return Object.values(this.filterForm.value).every(value => value === null || value === '');
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
