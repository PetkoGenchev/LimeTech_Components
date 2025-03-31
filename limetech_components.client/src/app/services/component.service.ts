import { Component, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ComponentDTO } from '../models/component.dto';


@Injectable({
  providedIn: 'root'
})
export class ComponentService {
  private apiUrl = 'https://localhost:7039/api/home';

  constructor(private http: HttpClient) { }

  // Add a component to the database
  // NOT DONE - Used to add new components, for ComponentController and add-component.component.ts 
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
  // NOT DONE - Used to edit components, for ComponentController and edit-component.component.ts
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
  // NOT DONE - Used to edit components, for ComponentController and edit-component.component.ts
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



  getComponents(filters: any): Observable<ComponentDTO> {
    return this.http.get<ComponentDTO>(`${this.apiUrl}/components`, { params: filters })
      .pipe(
        catchError(error => {
          console.error('Error fetching filtered components:', error);
          return throwError(() => new Error('Failed to fetch filtered components.'));
        })
      );
  }



  getComponentById(id: number): Observable<ComponentDTO> {
    return this.http.get<ComponentDTO>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(error => {
          console.error('Error fetching top components:', error);
          return throwError(() => new Error('Failed to fetch top components.'));
        })
      );
  }


  getAllComponents(sortBy: string = ''): Observable<ComponentDTO[]> {
    let params = new HttpParams();
    if (sortBy) {
      params = params.set('sortBy', sortBy);
    }

    return this.http.get<ComponentDTO[]>(`${this.apiUrl}/all-components`, { params })
      .pipe(
        catchError(error => {
          console.error('Error fetching all components:', error);
          return throwError(() => new Error('Failed to fetch all components.'));
        })
      );
  }

  getComponentsSortedByYear(): Observable<ComponentDTO[]> {
    return this.http.get<ComponentDTO[]>(`${this.apiUrl}/components-by-year`);
  }


}
