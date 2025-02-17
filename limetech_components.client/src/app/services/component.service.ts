import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ComponentDTO } from '../models/component.dto';


@Injectable({
  providedIn: 'root'
})
export class ComponentService {
  private apiUrl = 'http://localhost:7039/api/home';

  constructor(private http: HttpClient) { }

  // Add a component to the database
  addComponent(component: ComponentDTO): Observable<ComponentDTO> {
    if (!component) {
      throw new Error('Component data missing.');
    }
    return this.http.post<ComponentDTO>(`${this.apiUrl}`, component)
      .pipe(
        catchError(error => {
          console.error('Error adding component:', error);
          return throwError(() => new Error('Failed to add component.'));
        })
      );
  }


  // Edit a component in the database
  editComponent(id: number, component: ComponentDTO): Observable<ComponentDTO> {
    if (!id || id <= 0) {
      throw new Error('Invalid component ID.');
    }
    if (!component) {
      throw new Error('Component data missing.');
    }
    return this.http.put<ComponentDTO>(`${this.apiUrl}/${id}`, component)
      .pipe(
        catchError(error => {
          console.error('Error editing component:', error);
          return throwError(() => new Error('Failed to edit component.'));
        })
      );
  }


  // Change visibility of a component
  toggleVisibility(id: number, isPublic: boolean): Observable<void> {
    if (!id || id <= 0) {
      throw new Error('Invalid component ID.');
    }
    return this.http.patch<void>(`${this.apiUrl}/${id}/visibility`, { isPublic })
      .pipe(
        catchError(error => {
          console.error('Error changing visibility:', error);
          return throwError(() => new Error('Failed to change visibility.'));
        })
      );
  }

  // Get the top 8 components based on purchasedCount
  getTopComponents(): Observable<ComponentDTO[]> {
    return this.http.get<ComponentDTO[]>(`${this.apiUrl}/components`)
      .pipe(
        catchError(error => {
          console.error('Error fetching top components:', error);
          return throwError(() => new Error('Failed to fetch top components.'));
        })
      );
  }



  // Get all components with filters
  getComponents(filters: any): Observable<ComponentDTO[]> {
    let params = new HttpParams();
    Object.keys(filters).forEach(key => {
      if (filters[key] !== null && filters[key] !== '') {
        params = params.set(key, filters[key]);
      }
    });

    return this.http.get<ComponentDTO[]>(`${this.apiUrl}/components`, { params }).pipe(
      catchError(error => {
        console.error('Error fetching components', error);
        return throwError(() => new Error('Failed to fetch components'));
      })
    );
  }

  getComponentById(id: number): Observable<ComponentDTO> {
    return this.http.get<ComponentDTO>(`${this.apiUrl}/${id}`);
  }


}
