import { ToDoItem } from "./to-do-item.model";

export class ToDoList {
    public id!: string;
    public title: string = "";
    public reminderDate!: Date;
    public items: ToDoItem[] = [];
    public position!: number;
    public reminded: boolean = false;

}
