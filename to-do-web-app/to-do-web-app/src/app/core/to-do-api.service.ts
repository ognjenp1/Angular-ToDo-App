import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ToDoList } from '../models/to-do-list.model';
import { ToDoItem } from '../models/to-do-item.model';

@Injectable({
  providedIn: 'root'
})
export class ToDoApiService {
  readonly apiUrl = environment.apiUrl;

  constructor(private http:HttpClient) { }

  getToDoLists():Observable<ToDoList[]> {
    return this.http.get<ToDoList[]>(this.apiUrl);
  }

  deleteToDoList(id: string) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getToDoListById(id: string):Observable<ToDoList> {
    return this.http.get<ToDoList>(`${this.apiUrl}/${id}`);
  }

  updateToDoItem(listId: string, itemId: string, toDoItem: ToDoItem) {
    return this.http.put(`${this.apiUrl}/${listId}/to-do-items/${itemId}`, toDoItem);
  }

  addToDoItem(listId: string,  toDoItem: ToDoItem) {
    return this.http.post(`${this.apiUrl}/${listId}/to-do-items`, toDoItem);
  }

  deleteItemById(listId: string, itemId: string) {
    return this.http.delete(`${this.apiUrl}/${listId}/to-do-items/${itemId}`);
  }

  addToDoList(toDoList: ToDoList):Observable<ToDoList> {
    return this.http.post<ToDoList>(`${this.apiUrl}`, toDoList);
  }

  updateToDoList(toDoList: ToDoList) {
    return this.http.put(`${this.apiUrl}/${toDoList.id}`, toDoList);
  }

  updateItemPosition(toDoListId: string, toDoItemId: string, position: number) {
    return this.http.put(`${this.apiUrl}/${toDoListId}/to-do-items/${toDoItemId}/update-position`, position);
  }

  updateListPosition(toDoListId: string, position: number) {
    return this.http.put(`${this.apiUrl}/${toDoListId}/update-position`, position);
  }

  searchToDoLists(text: string) {
    return this.http.get<ToDoList[]>(`${this.apiUrl}/search/${text}`);
  }
}
