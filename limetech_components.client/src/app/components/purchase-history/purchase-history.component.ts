import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PurchaseHistoryDTO } from '../../models/purchase-history.dto';
import { PurchaseHistoryService } from '../../services/purchase-history.service';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormArray, FormGroup, ReactiveFormsModule, FormControl } from '@angular/forms';

@Component({
  selector: 'app-purchase-history',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './purchase-history.component.html',
  styleUrl: './purchase-history.component.css'
})
export class PurchaseHistoryComponent implements OnInit {
  purchaseHistory: PurchaseHistoryDTO[] = [];
  filteredPurchases: PurchaseHistoryDTO[] = [];
  customerId: string = '';


  // Filter dropdown
  dateFilterControl = new FormControl('30'); // Default: Last 30 days

  filterOptions = [
    { label: 'Last 30 Days', value: '30' },
    { label: 'Last 3 Months', value: '90' },
    { label: 'Last 6 Months', value: '180' },
    { label: 'Last 1 Year', value: '365' },
    { label: 'All Time', value: 'all' }
  ];


  constructor(
    private fb: FormBuilder,
    private purchaseHistoryService: PurchaseHistoryService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    const storedCustomerId = this.authService.getCustomerId();
    this.customerId = storedCustomerId ?? '';

    if (!this.customerId) {
      console.error("Customer ID is missing!");
      return;
    }

    this.loadPurchaseHistory();

    // Refresh list every time the filter changes
    this.dateFilterControl.valueChanges.subscribe(() => {
      this.applyFilter();
    });
  }

  loadPurchaseHistory(): void {
    this.purchaseHistoryService.getPurchaseHistory().subscribe({
      next: (data) => {
        this.purchaseHistory = data;
        this.applyFilter();
      },
      error: (error) => console.error('Failed to load purchase history', error),
    });
  }


  applyFilter(): void {
    const selectedValue = this.dateFilterControl.value;

    if (selectedValue === 'all') {
      this.filteredPurchases = [...this.purchaseHistory];
    } else {
      const days = parseInt(selectedValue ?? '30', 10);
      const cutoffDate = new Date();
      cutoffDate.setDate(cutoffDate.getDate() - days);

      this.filteredPurchases = this.purchaseHistory.filter(purchase => {
        const purchaseDate = new Date(purchase.purchaseDate);
        return purchaseDate >= cutoffDate;
      });
    }

    // Sort by most recent first
    this.filteredPurchases.sort((a, b) => new Date(b.purchaseDate).getTime() - new Date(a.purchaseDate).getTime());
  }


}
