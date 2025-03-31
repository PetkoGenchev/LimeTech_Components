import { Injectable } from '@angular/core';
import { BehaviorSubject, debounceTime, switchMap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import id from '@angular/common/locales/id';
import { ComponentDTO } from '../models/component.dto';

@Injectable({
  providedIn: 'root',
})
export class SearchService {

  private apiUrl = 'https://localhost:7039/api/home';

  private searchKeywordSubject = new BehaviorSubject<string>('');
  searchKeyword$ = this.searchKeywordSubject.asObservable().pipe(debounceTime(500));

  constructor(private http: HttpClient) { }

  updateSearchKeyword(keyword: string): void {
    this.searchKeywordSubject.next(keyword);
  }


  seachComponents(query: string) {
    return this.http.get<ComponentDTO[]>(`${this.apiUrl}/search?query=${encodeURIComponent(query)}`);
  }
}
