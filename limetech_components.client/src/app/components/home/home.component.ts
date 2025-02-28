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
      // If a producer is selected, filter the available categories (types of products)
      const filteredComponents = this.components.filter(c => c.producer === selectedProducer);
      this.filteredCategories = [...new Set(filteredComponents.map(c => c.typeOfProduct).filter((type): type is string => type !== null))];
    }
    else if (!selectedProducer && selectedType) {
      // If a category (type) is selected, filter the available producers
      const filteredComponents = this.components.filter(c => c.typeOfProduct === selectedType);
      this.filteredProducers = [...new Set(filteredComponents.map(c => c.producer).filter((producer): producer is string => producer !== null))];
    }
    else if (selectedProducer && selectedType) {
      // If both are selected, only show valid producers and categories based on selection
      const filteredComponents = this.components.filter(c => c.producer === selectedProducer && c.typeOfProduct === selectedType);
      this.filteredProducers = [...new Set(filteredComponents.map(c => c.producer).filter((producer): producer is string => producer !== null))];
      this.filteredCategories = [...new Set(filteredComponents.map(c => c.typeOfProduct).filter((type): type is string => type !== null))];
    }
    else {
      // If nothing is selected, reset to all available options
      this.filteredProducers = [...new Set(this.components.map(c => c.producer).filter((producer): producer is string => producer !== null))];
      this.filteredCategories = [...new Set(this.components.map(c => c.typeOfProduct).filter((type): type is string => type !== null))];
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
