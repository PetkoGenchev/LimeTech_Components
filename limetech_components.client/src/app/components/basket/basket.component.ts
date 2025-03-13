import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormArray, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { BasketDTO } from '../../models/basket.dto';
import { BasketService } from '../../services/basket.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-basket',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.css'
})
export class BasketComponent implements OnInit {
  basket: BasketDTO[] = [];
  basketForm: FormGroup;
  customerId: string = '';


  constructor(
    private fb: FormBuilder,
    private basketService: BasketService,
    private authService: AuthService
  )
  {
    this.basketForm = this.fb.group({
      selectedItems: this.fb.array([]),
    });
  }


  ngOnInit(): void {
    const storedCustomerId = this.authService.getCustomerId();
    this.customerId = storedCustomerId ?? '';

    if (!this.customerId) {
      console.error("Customer ID is missing!");
      return;
    }

    this.loadBasket();

    //this.basketService.getBasket(this.customerId).subscribe({
    //  next: (basket) => this.basket = basket,
    //  error: (error) => console.error("Error fetching basket:", error)
    //});

  }


  get selectedItems(): FormArray {
    return this.basketForm.get('selectedItems') as FormArray;
  }


  loadBasket(): void {

    //CHECKING IF I RECEIVE CUSTOMERID, DELETE IF WORKING
    console.log('Customer ID:', this.customerId);

    if (!this.customerId) {
      console.error('Customer ID is missing!');
      return;
    }

    this.basketService.getBasket(this.customerId).subscribe({
      next: (data) => {
        this.basket = data;
        this.populateForm();
      },
      error: (error) => console.error('Failed to load basket', error),
    });
  }


  populateForm(): void {
    this.selectedItems.clear();
    this.basket.forEach(() => {
      this.selectedItems.push(this.fb.control(false));
    });
  }


  removeFromBasket(index: number, componentId: number): void {
    this.basketService.removeFromBasket(this.customerId, componentId).subscribe({
      next: () => {
        this.basket.splice(index, 1);
        this.selectedItems.removeAt(index);
      },
      error: (error) => console.error('Failed to remove item from basket', error),
    });
  }


  clearBasket(): void {
    this.basketService.clearBasket(this.customerId).subscribe({
      next: () => {
        this.basket = [];
        this.selectedItems.clear();
      },
      error: (error) => console.error('Failed to clear basket', error),
    });
  }


  toggleSelection(index: number): void {
    const control = this.selectedItems.at(index);
    control.setValue(!control.value);
  }


  purchaseSelected(): void {
    const selectedComponents = this.basket
      .map((item, index) => (this.selectedItems.at(index).value ? item.componentId : null))
      .filter(id => id !== null);

    if (selectedComponents.length > 0) {
      console.log('Purchasing:', selectedComponents);
      this.basketService.purchaseBasket(this.customerId).subscribe(() => {
        this.loadBasket();
      });
    } else {
      console.warn('No items selected for purchase.');
    }
  }
}
