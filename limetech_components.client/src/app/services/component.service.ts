import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ComponentDTO } from '../models/component.dto';

@Injectable({
  providedIn: 'root'
})
export class ComponentService {
  private apiUrl = '/api/component';

  constructor(private http: HttpClient) { }

  // Add a component to the database
  addComponent(component: ComponentDTO): Observable<ComponentDTO> {
    return this.http.post<ComponentDTO>(`${this.apiUrl}`, component);
  }

  // Edit a component in the database
  editComponent(id: number, component: ComponentDTO): Observable<ComponentDTO> {
    return this.http.put<ComponentDTO>(`${this.apiUrl}/${id}`, component);
  }

  // Change visibility of a component
  changeVisibility(id: number, isPublic: boolean): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/visibility`, { isPublic });
  }

  // Get the top 8 components based on purchasedCount
  getTopComponents(): Observable<ComponentDTO[]> {
    return this.http.get<ComponentDTO[]>(`${this.apiUrl}/top?count=8`);
  }

  // Get all components with filters
  getComponents(filters: any): Observable<ComponentDTO[]> {
    return this.http.get<ComponentDTO[]>(`${this.apiUrl}`, { params: filters });
  }



}
