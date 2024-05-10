using Microsoft.EntityFrameworkCore;
using WebApplicationTodoList.Models;
using WebApplicationTodoList.ViewModels;

namespace WebApplicationTodoList.Services
{
    public interface ITodoService
    {
        Task<List<TodoEntry>> GetAll();
        Task<List<TodoEntry>> FilterByName(string name);
        Task<bool> AddTodo(TodoEntry entry);
        Task<bool> RemoveTodo(Guid id);
        Task<bool> UpdateTodo(Guid todoId, TodoEntryViewModel entry);
        Task<bool> UpdateStatusTodo(Guid todoId, string entry);
    }
}
