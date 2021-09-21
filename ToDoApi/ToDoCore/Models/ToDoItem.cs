using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoCore.Models
{
    public class ToDoItem
    { 
        public Guid Id { get; set; }
        public string Description { get; set; }
        public ToDoList ToDoList { get; set; }
        public Guid ToDoListId { get; set; }
        public bool IsCompleted { get; set; }

        public int Position { get; set; }
        public string Owner { get; set; }

        public ToDoItem(string description)
        {
            Description = description;
        }

        public void Update(ToDoItem item)
        {
            Position = item.Position;
            Description = item.Description;
            IsCompleted = item.IsCompleted;
        }

    }
}
