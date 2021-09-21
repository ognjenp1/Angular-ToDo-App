import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { ToDoApiService } from 'src/app/core/to-do-api.service';
import { ToDoListCreateService } from 'src/app/core/to-do-list-create.service';
import { ToDoItem } from 'src/app/models/to-do-item.model';
import { ToDoList } from 'src/app/models/to-do-list.model';

@Component({
  selector: 'app-to-do-item',
  templateUrl: './to-do-item.component.html',
  styleUrls: ['./to-do-item.component.css']
})
export class ToDoItemComponent implements OnInit {

  @Output() newItemAddedEvent = new EventEmitter<ToDoItem>();
  @Input() toDoListId!: string;
  @Input() toDoItem: ToDoItem = new ToDoItem();
  toDoList: ToDoList = new ToDoList();
  subscription!: Subscription;
  
  constructor(private apiService: ToDoApiService, private createService: ToDoListCreateService, private toastrService: ToastrService) { 
      this.subscription = createService.getListId().subscribe(data => {
        this.toDoListId = data.text;
        this.apiService.addToDoItem(this.toDoListId, this.toDoItem)
          .subscribe(() => {
            this.newItemAddedEvent.emit(this.toDoItem);
            this.toDoItem = new ToDoItem();
          }
          );
      });
    }

  ngOnInit(): void {
  }

  handleFocusOut(event: any) {
    if (event.target.value != '') {
      if (this.toDoListId != null) {
        if (!this.toDoItem.id) {
          this.apiService.addToDoItem(this.toDoListId, this.toDoItem).subscribe(() => {
            this.newItemAddedEvent.emit(this.toDoItem);
            this.toDoItem = new ToDoItem();
            this.toastrService.success("New ToDoItem added!");
          }, error => {
            this.toastrService.error("Error while adding new ToDoItem!");
          });
        } else {
          this.toDoItem.description = event.target.value;
          this.apiService.updateToDoItem(this.toDoListId, this.toDoItem.id, this.toDoItem).subscribe(() => 
            this.toastrService.success("ToDoItem updated!"),
            error => this.toastrService.error("Error while updating ToDoItem!")
          );
        }
      } else {
        this.createService.createToDoList(this.toDoList);
      }
    } else { }
  }

  handleChange() {
    this.toDoItem.isCompleted = !this.toDoItem.isCompleted;
    this.apiService.updateToDoItem(this.toDoListId, this.toDoItem.id, this.toDoItem).subscribe(() =>
      this.toastrService.success("ToDoItem updated")
    );
  }
}
