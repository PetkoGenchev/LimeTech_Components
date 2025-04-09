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



  getComponents(filters: { [key: string]: any }) {
    let params = new HttpParams();
    Object.keys(filters).forEach(key => {
      if (filters[key] !== null && filters[key] !== undefined) {
        params = params.set(key, filters[key]);
      }
    });

    return this.http.get<ComponentDTO[]>(`${this.apiUrl}/components`, { params });
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

  getComponentsSortedByYear(): Observable<ComponentDTO[]> {
    return this.http.get<ComponentDTO[]>(`${this.apiUrl}/components-by-year`);
  }


}
