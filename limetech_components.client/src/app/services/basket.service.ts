import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BasketDTO } from '../models/basket.dto';

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

  removeFromBasket(componentId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/basket/${componentId}`);
  }

  clearBasket(customerId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${customerId}/basket`);
  }

  purchaseBasket(customerId: string, selectedComponents: number[]): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${customerId}/purchase`, { componentIds: selectedComponents });
  }
}
