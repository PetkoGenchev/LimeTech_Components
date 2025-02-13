import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { BasketService } from '../../services/basket.service';
import { ComponentService } from '../../services/component.service';
import { ComponentDTO } from '../../models/component.dto';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  @Input() searchKeyword: string = '';

  components: ComponentDTO[] = [];
  topPurchased: ComponentDTO[] = [];
  filterForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private componentService: ComponentService,
    private basketService: BasketService
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
  }


  ngOnChanges(): void {
    this.loadComponents(); // Reload components whenever searchKeyword changes
  }

  //onFilterChange(): void {

  //  this.loadComponents();
  //  if (this.filtersApplied()) {
  //    this.loadComponents();
  //  } else {
  //    this.loadTopPurchased();
  //  }
  //}

  loadComponents(): void {

    const filters = {
      ...this.filterForm.value,
      keyword: this.searchKeyword  // Use searchKeyword from navbar
    };


    this.componentService.getComponents(filters).subscribe({
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
