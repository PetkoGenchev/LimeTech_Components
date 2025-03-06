import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PurchaseHistoryDTO } from '../../models/purchase-history.dto';
import { PurchaseHistoryService } from '../../services/purchase-history.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-purchase-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './purchase-history.component.html',
  styleUrl: './purchase-history.component.css'
})
export class PurchaseHistoryComponent implements OnInit {
  purchaseHistory: PurchaseHistoryDTO[] = [];
  customerId: string = '';

  constructor(
    private purchaseHistoryService: PurchaseHistoryService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.customerId = this.authService.getCustomerId();
    if (this.customerId) {
      this.loadPurchaseHistory();
    }
  }

  loadPurchaseHistory(): void {
    this.purchaseHistoryService.getPurchaseHistory(this.customerId).subscribe({
      next: (data) => {
        this.purchaseHistory = data;
      },
      error: (error) => console.error('Failed to load purchase history', error),
    });
  }
}
