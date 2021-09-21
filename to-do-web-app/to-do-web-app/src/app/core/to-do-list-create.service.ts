import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { ToDoApiService } from './to-do-api.service';
import { ToDoItem } from '../models/to-do-item.model';
import { ToDoList } from '../models/to-do-list.model';

@Injectable({
  providedIn: 'root'
})
export class ToDoListCreateService {
  private subject = new Subject<any>();

  createToDoList(list: ToDoList) {
    return this.service.addToDoList(list).subscribe(data => {
      this.subject.next({text: data.id });
    });
  }

  getListId() : Observable<any> {
    return this.subject.asObservable();
  }

  constructor(private service: ToDoApiService) { }
}
