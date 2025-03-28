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

  showNotification = false;
  countOfSelected = 0;
  totalPrice = 0;

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

  }


  get selectedItems(): FormArray {
    return this.basketForm.get('selectedItems') as FormArray;
  }

  get hasSelectedItems(): boolean {
    return this.basketForm.get('selectedItems')?.value.some((selected: boolean) => selected);
  }

  loadBasket(): void {
    if (!this.customerId) {
      console.error('Customer ID is missing!');
      return;
    }

    this.basketService.getBasket().subscribe({
      next: (data) => {
        this.basket = data;
        this.populateForm();
      },
      error: (error) => console.error('Failed to load basket', error),
    });
  }


  populateForm(): void {
    this.selectedItems.clear(); // Clear existing controls to avoid duplicates

    if (!this.basket || this.basket.length === 0) return;

    this.basket.forEach((item, index) => {
      this.selectedItems.push(this.fb.control(false)); // Initialize with default value
      console.log(`Added control at index ${index}`);
    });

    console.log("Form array populated with", this.selectedItems.length, "controls");
    console.log('Basket length:', this.basket.length);
  }



  removeSelectedFromBasket(): void {
    const selectedComponents = this.basket
      .map((item, index) => (this.selectedItems.at(index).value ? item.componentId : null))
      .filter(id => id !== null) as number[];

    if (selectedComponents.length > 0) {
      console.log("Removing items from basket:", selectedComponents);

      this.basketService.removeFromBasket(selectedComponents).subscribe({
        next: () => {
          this.basket = this.basket.filter((item, index) => !this.selectedItems.at(index).value);
          this.selectedItems.clear();
          this.basket.forEach(() => this.selectedItems.push(this.fb.control(false)));

          console.log("Selected items removed from basket.");
        },
        error: (error) => console.error("Failed to remove selected items from basket", error),
      });
    } else {
      console.warn("No items selected for removal.");
    }
  }

  purchaseSelected(): void {
    const selectedComponents = this.basket
      .map((item, index) => (this.selectedItems.at(index).value ? item.componentId : null))
      .filter(id => id !== null);

    if (selectedComponents.length > 0) {
      console.log('Purchasing:', selectedComponents);

      this.basketService.purchaseBasket(selectedComponents).subscribe({
        next: () => {
          // Show purchase confirmation message
          this.countOfSelected = selectedComponents.length;
          this.totalPrice = this.basket
            .filter((item, index) => this.selectedItems.at(index).value)
            .reduce((sum, item) => sum + item.pricePerUnit * item.quantity, 0);

          this.showNotification = true;

          setTimeout(() => this.showNotification = false, 3000);

          // Reload basket to reflect removed items
          this.loadBasket();
        },
        error: (error) => console.error('Failed to purchase items', error)
      });
    } else {
      console.warn('No items selected for purchase.');
    }
  }


  toggleSelection(index: number): void {
    const control = this.selectedItems.at(index);
    control.setValue(!control.value);
  }


  increaseQuantity(index: number): void {
    if (this.basket[index].quantity < 10) {
      this.basket[index].quantity++;
      this.updateTotalPrice(index);
    }
  }

  decreaseQuantity(index: number): void {
    if (this.basket[index].quantity > 1) {
      this.basket[index].quantity--;
      this.updateTotalPrice(index);
    }
  }

  updateTotalPrice(index: number): void {
    this.basket[index].totalPrice = this.basket[index].quantity * this.basket[index].pricePerUnit;
  }

  get totalSelectedPrice(): number {
    return this.basket
      .filter((_, i) => this.selectedItems.at(i).value)
      .reduce((sum, item) => sum + item.totalPrice, 0);
  }
}
