import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormArray, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { BasketDTO } from '../../models/basket.dto';
import { BasketService } from '../../services/basket.service';
import { Router } from '@angular/router';

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


  constructor(
    private fb: FormBuilder,
    private basketService: BasketService,
    private router: Router)
  {
    this.basketForm = this.fb.group({
      selectedItems: this.fb.array([]),
    });
  }


  ngOnInit(): void {
    this.loadBasket();
  }


  get selectedItems(): FormArray {
    return this.basketForm.get('selectedItems') as FormArray;
  }


  loadBasket(): void {
    this.basketService.getBasket().subscribe({
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
    this.basketService.removeFromBasket(componentId).subscribe({
      next: () => {
        this.basket.splice(index, 1);
        this.selectedItems.removeAt(index);
      },
      error: (error) => console.error('Failed to remove item from basket', error),
    });
  }


  clearBasket(): void {
    this.basketService.clearBasket().subscribe({
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
      this.basketService.purchaseBasket().subscribe({
        next: () => {
          console.log('Purchase successful!');
          this.loadBasket(); // Refresh basket
          this.router.navigate(['/purchase-history']); // Redirect to purchase history
        },
        error: (error) => console.error('Purchase failed', error),
      });
    } else {
      console.warn('No items selected for purchase.');
    }
  }
}
