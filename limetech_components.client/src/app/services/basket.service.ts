import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BasketDTO } from '../models/basket.dto';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  private apiUrl = 'https://localhost:7039/api/basket';

  constructor(private http: HttpClient) { }

  getBasket(): Observable<BasketDTO[]> {
    return this.http.get<BasketDTO[]>(`${this.apiUrl}`);
  }

  addToBasket(componentId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/add`, { componentId });
  }

  removeFromBasket(componentId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/remove/${componentId}`);
  }

  clearBasket(): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/clear`);
  }

  purchaseBasket(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/purchase`, {}); // Adjust the endpoint accordingly
  }
}
