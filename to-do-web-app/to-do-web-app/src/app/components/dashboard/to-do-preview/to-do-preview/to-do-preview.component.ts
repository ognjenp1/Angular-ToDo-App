import { Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import { ToDoList } from 'src/app/models/to-do-list.model';
import { ToDoApiService } from 'src/app/core/to-do-api.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-to-do-preview',
  templateUrl: './to-do-preview.component.html',
  styleUrls: ['./to-do-preview.component.css']
})
export class ToDoPreviewComponent implements OnInit {
  @Output() itemDeletedEvent = new EventEmitter<boolean>();
  @Input()
  toDoList!: ToDoList;

  constructor(private service: ToDoApiService, private router: Router, private toastrService: ToastrService) {
  }

  ngOnInit(): void {
  }

  deleteToDoList(id: string) {
    this.service.deleteToDoList(id).subscribe(
      () => {
        this.itemDeletedEvent.emit();
        this.toastrService.success("ToDoList succesfully deleted!");
      },
      error => this.toastrService.error("Error occured while deleting ToDoList!")
    );
  }

  redirectById(id: string) {
    this.router.navigateByUrl(`/to-do-lists/${id}`);
  }

  get completed(){
    return this.toDoList.items.filter(x => x.isCompleted).sort((a, b) => a.position - b.position);
  }

  get notCompleted(){
    return this.toDoList.items.filter(x => !x.isCompleted).sort((a, b) => a.position - b.position);
  }

}