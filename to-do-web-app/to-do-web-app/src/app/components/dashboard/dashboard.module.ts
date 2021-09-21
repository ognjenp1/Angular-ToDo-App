import { CommonModule } from "@angular/common";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { DashboardComponent } from "./dashboard.component";
import { ToDoPreviewComponent } from './to-do-preview/to-do-preview/to-do-preview.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { DragDropModule } from '@angular/cdk/drag-drop';


@NgModule({
    declarations:[
        DashboardComponent,
        ToDoPreviewComponent
    ],
    imports:[
        CommonModule,
        HttpClientModule,
        MatButtonModule,
        MatIconModule,
        DragDropModule
    ],
    exports:[
        DashboardComponent
    ]
})
export class DashboardModule { }