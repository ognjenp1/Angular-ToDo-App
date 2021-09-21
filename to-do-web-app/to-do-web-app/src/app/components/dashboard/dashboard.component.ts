import { CdkDrag, CdkDragDrop } from '@angular/cdk/drag-drop';
import { Component, OnInit, AfterViewChecked, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ToDoApiService } from 'src/app/core/to-do-api.service';
import { ToDoList } from 'src/app/models/to-do-list.model';
import { SearchService } from 'src/app/core/search.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  toDoLists: ToDoList[] = [];
  subscription: Subscription;

  constructor(
    private service: ToDoApiService,
    private toastrService: ToastrService,
    private searchService: SearchService
  ) {
    this.subscription = this.searchService
      .getSearchText()
      .subscribe((message) => {
        if (message.text != '') {
          this.service.searchToDoLists(message.text).subscribe((data) => {
            this.toDoLists = data;
          });
        } else if (message.text == '' || message == undefined) {
          this.getAllToDoLists();
        }
      });
  }

  ngOnInit(): void {
    this.getAllToDoLists();
  }

  getAllToDoLists() {
    this.service.getToDoLists().subscribe((data) => {
      this.toDoLists = data;
    });
  }

  listReminded(remindedList: any) {
    this.service.updateToDoList(remindedList).subscribe(
      () => {
        this.toastrService.success('ToDoList reminded!');
      },
      (error) => {
        this.toastrService.error('Error while reminding ToDoList!');
      }
    );
  }

  dropNotReminded(event: CdkDragDrop<ToDoList[]>) {
    this.service
      .updateListPosition(
        this.notReminded[event.previousIndex].id,
        this.notReminded[event.currentIndex].position
      )
      .subscribe(() => {
        this.getAllToDoLists();
      });
  }

  dropReminded(event: CdkDragDrop<ToDoList[]>) {
    this.service
      .updateListPosition(
        this.reminded[event.previousIndex].id,
        this.reminded[event.currentIndex].position
      )
      .subscribe(() => {
        this.getAllToDoLists();
      });
  }

  get notReminded() {
    return this.toDoLists
      .filter((x) => !x.reminded && x.reminderDate != undefined)
      .sort((a, b) => a.position - b.position);
  }

  get reminded() {
    return this.toDoLists
      .filter((x) => x.reminded || x.reminderDate == undefined)
      .sort((a, b) => a.position - b.position);
  }
}
