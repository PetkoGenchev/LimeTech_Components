import { Component, OnInit } from '@angular/core';
import { BasketService } from '../../services/basket.service';
import { ComponentService } from '../../services/component.service';
import { ComponentDTO } from '../../models/component.dto';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  components: ComponentDTO[] = [];
  topPurchased: ComponentDTO[] = [];
  filters = {
    keyword: '', // New keyword filter
    name: '',
    typeOfProduct: '',
    minPrice: null,
    maxPrice: null,
    productionYear: null,
    status: null,
    currentPage: 1,
    componentsPerPage: 10,
  };

  constructor(
    private componentService: ComponentService,
    private basketService: BasketService
  ) { }

  ngOnInit(): void {
    this.loadTopPurchased();
  }

  onFilterChange(): void {
    if (this.filtersApplied()) {
      this.loadComponents();
    } else {
      this.components = [];
    }
  }

  loadComponents(): void {
    this.componentService.getComponents(this.filters).subscribe({
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

  filtersApplied(): boolean {
    return Object.values(this.filters).some(value => value !== '' && value !== null);
  }
}
