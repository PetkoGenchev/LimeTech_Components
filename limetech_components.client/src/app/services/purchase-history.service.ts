import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PurchaseHistoryDTO } from '../models/purchase-history.dto';

@Injectable({
  providedIn: 'root'
})
export class PurchaseHistoryService {
  private apiUrl = 'https://localhost:7039/api/customer';

  constructor(private http: HttpClient) { }

  getPurchaseHistory(): Observable<PurchaseHistoryDTO[]> {
    return this.http.get<PurchaseHistoryDTO[]>(`${this.apiUrl}/purchases`);
  }
}
