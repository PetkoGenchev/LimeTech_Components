import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BasketDTO } from '../models/basket.dto';
import { PurchaseHistoryDTO } from '../models/purchase-history.dto';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  private apiUrl = 'https://localhost:7039/api/customer';

  constructor(private http: HttpClient) { }

  getBasket(): Observable<BasketDTO[]> {
    return this.http.get<BasketDTO[]>(`${this.apiUrl}/basket`);
  }

  addToBasket(componentId: number): Observable<void> {
    return this.http.post<void>(
      `${this.apiUrl}/basket`,
      { componentId },
      { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) }
    );
  }

  removeFromBasket(componentIds: number[]): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/basket/remove-multiple`, componentIds);
  }

  purchaseBasket(selectedComponents: number[]):
    Observable<{ message: string; purchasedItems: PurchaseHistoryDTO[] }> {
    return this.http.post<{ message: string; purchasedItems: PurchaseHistoryDTO[] }>
      (`${this.apiUrl}/purchase`, selectedComponents);
  }
}
