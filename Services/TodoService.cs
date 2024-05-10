using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using WebApplicationTodoList.Models;
using WebApplicationTodoList.ViewModels;

namespace WebApplicationTodoList.Services
{
    public class TodoService
    {
        private readonly ILogger<TodoService> _logger;
        private readonly WebApiDemoContext _Context;

        public TodoService(ILogger<TodoService> logger, WebApiDemoContext context)
        {
            _logger = logger;
            _Context = context;
        }

        public  List<TodoEntry> GetAll()
        {
            return _Context.TodoEntries.ToList();
        }

        public async Task<TodoEntry?> GetById(Guid id)
        {
            return await _Context.TodoEntries.FindAsync(id);
        }

        public async Task<List<TodoEntry>> FilterByName(string name)
        {
            return _Context.TodoEntries
                .Where(todo => todo.Title.Contains(name))
                .ToList();
        }

        public async Task<bool> AddTodo(TodoEntry entry)
        {
            if (entry == null) { return false; }
            else
            {
                if (_Context.TodoEntries.Any(existingTodo => existingTodo.Title == entry.Title))
                {
                    return false;
                }
                await _Context.TodoEntries.AddAsync(entry);
                await _Context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> RemoveTodo(Guid id)
        {
            var todo = await GetById(id);
            if (todo == null) { return false; }
            else
            {
                _Context.TodoEntries.Remove(todo);
                _Context.SaveChanges();
                return true;
            }
        }

        public async Task<bool> UpdateTodo(Guid todoId, TodoEntryViewModel entry)
        {
            using var transaction = await _Context.Database.BeginTransactionAsync();
            TodoEntry? todoEntry = _Context.TodoEntries.FirstOrDefault(t => t.Id == todoId);
            if (todoEntry == null)
            {
                return false;
            }

            //if (entry.Tags.Length > 0)
            //{
            //    var listOfTags = entry.Tags.Select(tag => new TodoTags { Name = tag });
            //    todoEntry.Tags.AddRange(listOfTags);
            //}

            try
            {
                if (entry.Title != null)
                {
                    todoEntry.Title = entry.Title;
                }
                if (entry.Description != null)
                {
                    todoEntry.Description = entry.Description;
                }
                if (entry.DueDate != null)
                {
                    todoEntry.DueDate = entry.DueDate;
                }
                if (entry.Status != null)
                {
                    todoEntry.Status = entry.Status;
                }
                todoEntry.UpdateDate = DateTime.Now;

                await _Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Update Todo Failed, Message: {error}", ex);
                return false;
            }

            //TodoEntry todoEntry = new TodoEntry(entry.Title, entry.Description, entry.DueDate);

            

            //try
            //{
            //    await _context.TodoEntries.AddAsync(todoEntry);
            //    await _context.SaveChangesAsync();
            //    await transaction.CommitAsync();

            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("Update Todo Failed, Message: {error}", ex);
            //    return false;
            //}
        }
        public async Task<bool> UpdateStatusTodo(Guid todoId, string entry)
        {
            using var transaction = await _Context.Database.BeginTransactionAsync();
            TodoEntry? todoEntry = _Context.TodoEntries.FirstOrDefault(t => t.Id == todoId);
            if (todoEntry == null)
            {
                return false;
            }

            try
            {
                if (entry != null)
                {
                    todoEntry.Status = entry;
                }
                await _Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Update Status Failed, Message: {error}", ex);
                return false;
            }
        }
    }
    
}

