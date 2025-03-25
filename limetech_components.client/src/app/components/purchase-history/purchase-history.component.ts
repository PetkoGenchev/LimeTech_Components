import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PurchaseHistoryDTO } from '../../models/purchase-history.dto';
import { PurchaseHistoryService } from '../../services/purchase-history.service';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormArray, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-purchase-history',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './purchase-history.component.html',
  styleUrl: './purchase-history.component.css'
})
export class PurchaseHistoryComponent implements OnInit {
  purchaseHistory: PurchaseHistoryDTO[] = [];
  customerId: string = '';

  constructor(
    private fb: FormBuilder,
    private purchaseHistoryService: PurchaseHistoryService,
    private authService: AuthService
  ){ }

  ngOnInit(): void {
    const storedCustomerId = this.authService.getCustomerId();
    this.customerId = storedCustomerId ?? '';

    if (!this.customerId) {
      console.error("Customer ID is missing!");
      return;
    }

    this.loadPurchaseHistory();

  }

  loadPurchaseHistory(): void {
    this.purchaseHistoryService.getPurchaseHistory().subscribe({
      next: (data) => {
        this.purchaseHistory = data;
      },
      error: (error) => console.error('Failed to load purchase history', error),
    });
  }
}
