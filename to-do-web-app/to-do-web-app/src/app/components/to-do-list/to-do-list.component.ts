import { Component, OnDestroy, OnInit, Output, EventEmitter} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToDoApiService } from 'src/app/core/to-do-api.service';
import { ToDoList } from 'src/app/models/to-do-list.model';
import { ToDoItem } from 'src/app/models/to-do-item.model';
import { Subscription } from 'rxjs';
import { ToDoListCreateService } from 'src/app/core/to-do-list-create.service';
import { ToastrService } from 'ngx-toastr';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { NavbarService } from 'src/app/core/navbar.service';


@Component({
  selector: 'app-to-do-list',
  templateUrl: './to-do-list.component.html',
  styleUrls: ['./to-do-list.component.css']
})
export class ToDoListComponent implements OnInit, OnDestroy {
  @Output() listRemindedEvent = new EventEmitter<ToDoList>();
  id: string = "";
  list : ToDoList = new ToDoList();
  subscription!: Subscription;

  constructor(private route: ActivatedRoute, private apiService: ToDoApiService, private createService: ToDoListCreateService, private toastrService: ToastrService,
    public nav: NavbarService) {
    this.subscription = this.createService.getListId().subscribe(listId => {
      if (listId) {
        this.getToDoListById(listId.text);
      } else {
        this.list.id = "";
      }
    })
  }

  drop(event: CdkDragDrop<ToDoItem[]>) {
    this.apiService.updateItemPosition(this.list.id, this.notCompleted[event.previousIndex].id, this.notCompleted[event.currentIndex].position).subscribe( () => {
      this.getToDoListById(this.list.id);
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.list.id = params['id'];
      this.getToDoListById(this.list.id);
      this.nav.hide();      
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  updateList() {
    this.apiService.updateToDoList(this.list).subscribe();
  }

  handleListTitle(event: any) {
    this.list.title = event.target.value;
    if (this.list.id == "" || this.list.id == undefined) {
      if (this.list.title != "") {
        this.apiService.addToDoList(this.list).subscribe(() => {  
          this.toastrService.success("New ToDoList added!");
        },
        error => this.toastrService.error("Error while creating ToDoList!")
        );
      }
    } else {
      this.apiService.updateToDoList(this.list).subscribe(() => 
        this.toastrService.success("ToDoList title updated!"),
        error => this.toastrService.error("Error while updating ToDoList title!")
      );
    }
  }

  handleListReminder() {
    if (new Date(this.list.reminderDate) >= new Date()) {
      this.list.reminded = false;
      this.apiService.updateToDoList(this.list).subscribe();  
    } else {
      this.toastrService.error("Invalid DateTime input!");
    }
  }

  addNewInput(item: ToDoItem) {
    this.getToDoListById(this.list.id);
  }

  getToDoListById(id: string) {
    this.apiService.getToDoListById(id).subscribe(
      data => {
        this.list = data;
        if (!this.list.reminded)
          this.toastrService.success("ToDoList reminded!");
        this.list.reminded = true;
        this.apiService.updateToDoList(this.list).subscribe();
      }
    );
  }

  get completed(){
    return this.list.items.filter(x => x.isCompleted).sort((a, b) => a.position - b.position);
  }

  get notCompleted(){
    return this.list.items.filter(x => !x.isCompleted).sort((a, b) => a.position - b.position);
  }
}
