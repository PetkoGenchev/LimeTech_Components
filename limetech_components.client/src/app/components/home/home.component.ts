import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { BasketService } from '../../services/basket.service';
import { ComponentService } from '../../services/component.service';
import { ComponentDTO } from '../../models/component.dto';
//import { ActivatedRoute } from '@angular/router';
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
  filterForm: FormGroup;

  /*  @Input() searchKeyword: string = '';*/
  searchKeyword = '';

  constructor(
    private fb: FormBuilder,
    private componentService: ComponentService,
    private basketService: BasketService,
    private searchService: SearchService,
    //private route: ActivatedRoute
  ) {
    this.filterForm = this.fb.group({
      name: [''],
      typeOfProduct: [''],
      minPrice: [null],
      maxPrice: [null],
      productionYear: [null],
      status: [null],
      //currentPage: 1,
      //componentsPerPage: 10,
    })
  }

  ngOnInit(): void {
    this.loadComponents();

    this.searchService.searchKeyword$.subscribe((keyword) => {
      this.searchKeyword = keyword;
      this.filterComponents();
    });

    // Listen for search query from the URL params (if needed)
    //this.route.queryParams.subscribe(params => {
    //  if (params['search']) {
    //    this.searchKeyword = params['search'];
    //    this.filterForm.patchValue({ name: this.searchKeyword });
    //    this.loadComponents();
    //  }
    //});
  }


  filterComponents(): void {
    console.log('Filtering components with:', this.searchKeyword);
    // Add filtering logic here
  }


  //ngOnChanges(): void {
  //  // Automatically filter components when searchKeyword changes
  //  if (this.searchKeyword) {
  //    this.filterForm.patchValue({ name: this.searchKeyword });
  //  }
  //  this.loadComponents();
  //}




  //ngOnChanges(): void {
  //  this.loadComponents(); // Reload components whenever searchKeyword changes
  //}

  //onFilterChange(): void {

  //  this.loadComponents();
  //  if (this.filtersApplied()) {
  //    this.loadComponents();
  //  } else {
  //    this.loadTopPurchased();
  //  }
  //}

  loadComponents(): void {
    this.componentService.getComponents(this.filterForm.value).subscribe({
      next: (data) => this.components = data,
      error: (error) => console.error('Failed to load components', error),
    });
  }

  loadTopPurchased(): void {
    this.componentService.getTopComponents().subscribe({
      next: (data) => this.topPurchased = data,
      error: (error) => console.error('Failed to load top purchased components', error),
    });
  }

  addToBasket(componentId: number): void {
    this.basketService.addToBasket(componentId).subscribe(() => {
      console.log(`Component ${componentId} added to basket.`);
    });
  }

  //filtersApplied(): boolean {
  //  const { currentPage, componentsPerPage, ...rest } = this.filters;
  //  return Object.values(rest).some(value => value !== '' && value !== null);
  //}
}
