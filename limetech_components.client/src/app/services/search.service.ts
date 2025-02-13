import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  private searchKeywordSubject = new BehaviorSubject<string>('');
  searchKeyword$ = this.searchKeywordSubject.asObservable();

  updateSearchKeyword(keyword: string): void {
    this.searchKeywordSubject.next(keyword);
  }
}
