import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private subject = new Subject<any>();

  sendSearchText(filter: string) {
    this.subject.next({ text : filter });
  }

  getSearchText() : Observable<any> {
    return this.subject.asObservable();
  }

  constructor() { }
}
