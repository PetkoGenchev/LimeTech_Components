import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormArray, FormGroup } from '@angular/forms';
import { BasketDTO } from '../../models/basket.dto';
import { BasketService } from '../../services/basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.css'
})
export class BasketComponent implements OnInit {
  basket: BasketDTO[] = [];
  basketForm: FormGroup;


  constructor(private fb: FormBuilder, private basketService: BasketService) {
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
    this.basket.forEach(item => {
      this.selectedItems.push(this.fb.control(false));
    });
  }


  removeFromBasket(index: number, componentId: number): void {
    this.basketService.removeFromBasket(componentId).subscribe(() => {
      this.basket.splice(index, 1);
      this.selectedItems.removeAt(index);
    });
  }

  clearBasket(): void {
    this.basketService.clearBasket().subscribe(() => {
      this.basket = [];
      this.selectedItems.clear();
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

      // Move selected items to purchase history (MAKE PURCHASE HISTORY)
      // For now, it just removes them from the basket
      selectedComponents.forEach(componentId => {
        const index = this.basket.findIndex(item => item.componentId === componentId);
        if (index !== -1) {
          this.removeFromBasket(index, componentId);
        }
      });
    } else {
      console.warn('No items selected for purchase.');
    }
  }
}
