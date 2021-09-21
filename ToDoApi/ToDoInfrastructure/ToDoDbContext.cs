using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoCore.Models;

namespace ToDoInfrastructure
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { }

        public DbSet<ToDoList> Lists { get; set; }
        public DbSet<ToDoItem> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<ToDoList>(new ToDoListConfiguration());
            modelBuilder.ApplyConfiguration<ToDoItem>(new ToDoItemConfiguration());
        }
    }
}
