import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ToDoListComponent } from './components/to-do-list/to-do-list.component';
import { LoadGuard } from './guards/load.guard';

const routes: Routes = [
  { 
    path: 'to-do-lists', 
    component: ToDoListComponent,
    canLoad: [LoadGuard]
  },
  { 
    path: 'to-do-lists/:id', 
    component: ToDoListComponent,
    canLoad: [LoadGuard]
  },
  { 
    path: '', 
    component: DashboardComponent,
    canLoad: [LoadGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
