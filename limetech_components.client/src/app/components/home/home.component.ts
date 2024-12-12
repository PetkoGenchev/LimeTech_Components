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
  ) {}

  ngOnInit(): void {
    this.loadComponents();
    this.loadTopPurchased();
    this.loadBasket();
  }

  //loadComponents(): void {
  //  this.componentService.getComponents(this.filters).subscribe((data) => {
  //    this.components = data;
  //  });
  //}

  //loadTopPurchased(): void {
  //  this.componentService.getTopPurchasedComponents().subscribe((data) => {
  //    this.topPurchased = data;
  //  });
  //}


  // Load all components with filters
  loadComponents(): void {
    this.componentService.getComponents(this.filters).subscribe({
      next: (data) => (this.components = data),
      error: (err) => console.error('Failed to load components', err),
    });
  }

  // Load the top 8 purchased components
  loadTopPurchased(): void {
    this.componentService.getTopComponents().subscribe({
      next: (data) => (this.topPurchased = data),
      error: (err) => console.error('Failed to load top purchased components', err),
    });
  }


  loadBasket(): void {
    this.basketService.getBasket().subscribe((data) => {
      this.basket = data;
    });
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

  // Add a new component
  addComponent(newComponent: ComponentDTO): void {
    this.componentService.addComponent(newComponent).subscribe({
      next: (component) => {
        console.log('Component added:', component);
        this.loadComponents(); // Refresh the list after adding
      },
      error: (err) => console.error('Failed to add component', err),
    });
  }

  // Edit an existing component
  editComponent(id: number, updatedComponent: ComponentDTO): void {
    this.componentService.editComponent(id, updatedComponent).subscribe({
      next: (component) => {
        console.log('Component updated:', component);
        this.loadComponents(); // Refresh the list after editing
      },
      error: (err) => console.error('Failed to edit component', err),
    });
  }

  // Change visibility of a component
  toggleVisibility(id: number, isPublic: boolean): void {
    this.componentService.changeVisibility(id, isPublic).subscribe({
      next: () => {
        console.log('Visibility updated');
        this.loadComponents(); // Refresh the list after updating
      },
      error: (err) => console.error('Failed to update visibility', err),
    });
  }
}
