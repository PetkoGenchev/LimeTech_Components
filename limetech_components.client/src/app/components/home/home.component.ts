import { Component, OnInit } from '@angular/core';
import { BasketService } from '../../services/basket.service';
import { BasketDTO } from '../../models/basket.dto';
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
  basket: BasketDTO[] = [];
  filters = {
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
    this.loadComponents();
    this.loadTopPurchased();
    this.loadBasket();
  }

  // Load all components with filters
  loadComponents(): void {
    this.componentService.getComponents(this.filters).subscribe({
      next: (data) => (this.components = data),
      error: (error) => console.error('Failed to load components', error),
    });
  }

  // Load the top 8 purchased components
  loadTopPurchased(): void {
    this.componentService.getTopComponents().subscribe({
      next: (data) => (this.topPurchased = data),
      error: (error) => console.error('Failed to load top purchased components', error),
    });
  }


  loadBasket(): void {
    this.basketService.getBasket().subscribe(
      (data) => (this.basket = data),
      (error) => console.error('Failed to load basket',error)
    );
  }

  addToBasket(componentId: number): void {
    this.basketService.addToBasket(componentId).subscribe(() => {
      this.loadBasket();
    });
  }

  removeFromBasket(componentId: number): void {
    this.basketService.removeFromBasket(componentId).subscribe(() => {
      this.loadBasket();
    });
  }

  clearBasket(): void {
    this.basketService.clearBasket().subscribe(() => {
      this.loadBasket();
    });
  }
}
