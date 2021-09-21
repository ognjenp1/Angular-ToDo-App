using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoCore.Models;

namespace ToDoCore.Interfaces
{
    public interface IToDoService
    {
        public IEnumerable<ToDoList> GetAllToDoLists(string owner);
        public ToDoList GetToDoListById(Guid toDoListId);
        public IEnumerable<ToDoList> SearchToDoLists(string text, string owner);
        public void AddToDoList(ToDoList toDoList, string owner);
        public void UpdateToDoList(Guid toDoListId, ToDoList updatedToDoList);
        public void DeleteToDoList(Guid toDoListId);
        public IEnumerable<ToDoItem> GetToDoItemsByListId(Guid toDoListId);
        public ToDoItem GetToDoItemById(Guid toDoListId, Guid toDoItemId);
        public void AddToDoItem(Guid toDoListId, ToDoItem toDoItem, string owner);
        public void UpdateToDoItem(Guid toDoListid, Guid toDoItemId, ToDoItem toDoItem);
        public void DeleteToDoItem(Guid toDoListId, Guid toDoItemId);
        void UpdateItemPosition(Guid toDoListId, Guid toDoItemId, int position);
        void UpdateListPosition(Guid toDoListId, int position);
    }
}
