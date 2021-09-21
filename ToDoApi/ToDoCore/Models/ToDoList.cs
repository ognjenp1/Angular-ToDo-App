using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoCore.Models
{
    public class ToDoList
    { 
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<ToDoItem> Items { get; set; }
        public DateTime ReminderDate { get; set; }
        public bool Reminded { get; set; }
        public int Position { get; set; }
        public string Owner { get; set; }

        public ToDoList()
        {
            Items = new List<ToDoItem>();
        }

        public void Update(ToDoList list)
        {
            Title = list.Title;
            Reminded = list.Reminded;
            ReminderDate = list.ReminderDate;
        }
    }
}
