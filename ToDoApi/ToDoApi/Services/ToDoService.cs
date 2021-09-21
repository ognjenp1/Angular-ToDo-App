using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoCore.Interfaces;
using ToDoCore.Models;
using ToDoInfrastructure;

namespace ToDoApi.Services
{
    public class ToDoService : IToDoService
    {
        private readonly ToDoDbContext _dbContext;

        public ToDoService(ToDoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddToDoItem(Guid toDoListId, ToDoItem toDoItem, string owner)
        {
            ToDoList list = GetToDoList(toDoListId);
            if (list != null)
            {
                ToDoItem item = GetToDoItem(toDoItem.Id);
                if (item == null)
                {
                    int numOfItems = _dbContext.Items.Where(i => i.ToDoListId == toDoListId).Count();
                    toDoItem.Position = numOfItems;
                    toDoItem.ToDoListId = toDoListId;
                    toDoItem.Owner = owner;
                    _dbContext.Items.Add(toDoItem);
                    _dbContext.SaveChanges();
                    return;
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void AddToDoList(ToDoList toDoList, string owner)
        {
            Guid id = toDoList.Id;
            ToDoList list = GetToDoList(id);
            if (list == null)
            {
                toDoList.Position = _dbContext.Lists.Count() + 1;
                toDoList.Owner = owner;
                _dbContext.Lists.Add(toDoList);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception();
            }
        }

        public void DeleteToDoItem(Guid toDoListId, Guid toDoItemId)
        {
            ToDoList list = GetToDoList(toDoListId);
            if (list != null)
            {
                ToDoItem item = GetToDoItem(toDoItemId);
                if (item != null)
                {
                    _dbContext.Remove(item);
                    _dbContext.Items.Where(l => l.ToDoListId == toDoListId && l.Position > item.Position).ToList().ForEach(l => l.Position--);
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void DeleteToDoList(Guid toDoListId)
        {
            ToDoList list = GetToDoList(toDoListId);
            if (list != null)
            {
                _dbContext.Remove(list);
                _dbContext.Lists.Where(l => l.Position > list.Position).ToList().ForEach(l => l.Position--);
                _dbContext.SaveChanges();
            }else
            {
                throw new KeyNotFoundException();
            }
        }

        public IEnumerable<ToDoList> GetAllToDoLists(string owner)
        {
            return _dbContext.Lists.Where(x => x.Owner == owner).Include(l => l.Items).OrderByDescending(l => l.Position).ToList();
        }

        public ToDoItem GetToDoItemById(Guid toDoListId, Guid toDoItemId)
        {
            ToDoList list = GetToDoList(toDoListId);
            if (list != null)
            {
                ToDoItem item = GetToDoItem(toDoItemId);
                if (item != null)
                    return item;
            }

            throw new KeyNotFoundException();
        }

        public IEnumerable<ToDoItem> GetToDoItemsByListId(Guid toDoListId)
        {
            ToDoList list = GetToDoList(toDoListId);
            if (list != null)
                return _dbContext.Items.Where(i => i.ToDoListId == toDoListId).OrderBy(i => i.Position).ToList();

            throw new KeyNotFoundException();
        }

        public ToDoList GetToDoListById(Guid toDoListId)
        {
            ToDoList list = GetToDoList(toDoListId);
            if (list != null)
                return list;

            throw new KeyNotFoundException();
        }

        public IEnumerable<ToDoList> SearchToDoLists(string text, string owner)
        {
            return _dbContext.Lists.Where(l => l.Owner == owner && l.Title.ToLower().Contains(text.ToLower()) || l.Items.Any(i => i.Description.Contains(text))).Include(l => l.Items).ToList();
        }

        public void UpdateToDoItem(Guid toDoListId, Guid toDoItemId, ToDoItem updatedToDoItem)
        {
            ToDoList list = GetToDoList(toDoListId);
            if (list != null)
            {
                ToDoItem item = GetToDoItem(toDoItemId);
                if (item != null)
                {
                    item.Update(updatedToDoItem);
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void UpdateToDoList(Guid toDoListId, ToDoList updatedToDoList)
        {
            ToDoList list = GetToDoList(toDoListId);
            if (list != null)
            {
                list.Update(updatedToDoList);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void UpdateItemPosition(Guid toDoListId, Guid toDoItemId, int position)
        {
            ToDoList list = GetToDoList(toDoListId);
            if (list != null)
            {
                ToDoItem item = GetToDoItem(toDoItemId);
                if (item != null)
                {
                    List<ToDoItem> items = _dbContext.Items.Where(i => i.ToDoListId == toDoListId).OrderBy(i => i.Position).ToList();

                    if (position < 0 || position > items.Count)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    else
                    {
                        if (position < item.Position)
                        {
                            items.Where(x => x.Position >= position && x.Position < item.Position).ToList().ForEach(x => x.Position++);
                            item.Position = position;
                            _dbContext.SaveChanges();
                        }
                        else if (position > item.Position)
                        {
                            items.Where(x => x.Position > item.Position && x.Position <= position).ToList().ForEach(x => x.Position--);
                            item.Position = position;
                            _dbContext.SaveChanges();
                        }
                        else { }
                    }
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void UpdateListPosition(Guid toDoListId, int position)
        {
            ToDoList list = GetToDoList(toDoListId);
            if (list != null)
            {
                List<ToDoList> lists = _dbContext.Lists.OrderBy(l => l.Position).ToList();
                if (position < 0 || position > lists.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    if (position < list.Position)
                    {
                        lists.Where(x => x.Position >= position && x.Position < list.Position).ToList().ForEach(x => x.Position++);
                        list.Position = position;
                        _dbContext.SaveChanges();
                    }
                    else if (position > list.Position)
                    {
                        lists.Where(x => x.Position > list.Position && x.Position <= position).ToList().ForEach(x => x.Position--);
                        list.Position = position;
                        _dbContext.SaveChanges();
                    }
                    else { }
                }
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        private ToDoItem GetToDoItem(Guid toDoItemId)
        {
            ToDoItem item = _dbContext.Items.Find(toDoItemId);
            return item;
        }

        private ToDoList GetToDoList(Guid toDoListId)
        {
            ToDoList list = _dbContext.Lists.Include(x => x.Items).SingleOrDefault(l => l.Id == toDoListId);
            return list;
        }

    }
}
