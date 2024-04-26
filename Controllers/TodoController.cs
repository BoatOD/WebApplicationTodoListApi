using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using WebApplicationTodoList.ViewModels;
using WebApplicationTodoList.Models;
using System.Linq;

namespace WebApplicationTodoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private static List<TodoEntry> todoList = new();
        private readonly ILogger<TodoController> _logger;

        private readonly WebApiDemoContext _Context;
        public TodoController(ILogger<TodoController> logger, WebApiDemoContext context)
        {
            _logger = logger;
            _Context = context;
        }

        [HttpGet]
        public ActionResult List()
        {
            return Ok(_Context.TodoEntries.ToList());
        }

        [HttpGet("filter")]
        public ActionResult Search([FromQuery] string name)
        {
            return Ok(_Context.TodoEntries
                .Where(todo => todo.Title.Contains(name))
                .ToList());
        }

        [HttpPost]
        public ActionResult Add([FromBody] TodoEntryViewModel entry)
        {
            var newTodoEntry = new TodoEntry(entry.Title, entry.Description, entry.DueDate);

            if (_Context.TodoEntries.Any(existingTodo => existingTodo.Id == newTodoEntry.Id))
            {
                return Conflict("Duplicated Todo Id");
            }

            _Context.TodoEntries.Add(newTodoEntry);
            _Context.SaveChanges();
            return Created($"/{newTodoEntry.Id}", entry);
        }

        [HttpDelete("{todoId}")]
        public ActionResult Remove([FromRoute] Guid todoId)
        {
            TodoEntry? dataToRemove = _Context.TodoEntries.FirstOrDefault(t => t.Id == todoId);
            if (dataToRemove == null)
            {
                return Conflict("This Todo Id doesn't exist.");
            } else
            {
                _Context.TodoEntries.Remove(dataToRemove);
                _Context.SaveChanges();
                return Ok($"Delete Succeed: {todoId.ToString()}");
            }
        }

        [HttpPut("{todoId}")]
        public ActionResult Replace([FromRoute] Guid todoId, [FromBody] TodoEntryViewModel entry)
        {
            TodoEntry? dataToUpdate = _Context.TodoEntries.FirstOrDefault(t => t.Id == todoId);

            if (dataToUpdate == null)
            {
                return Conflict("This Todo Id doesn't exist.");
            }
            else
            {
                if (entry.Title != null)
                {
                    dataToUpdate.Title = entry.Title;
                }
                if (entry.Description != null)
                {
                    dataToUpdate.Description = entry.Description;
                }
                if (entry.DueDate != null)
                {
                    dataToUpdate.DueDate = entry.DueDate;
                }
                dataToUpdate.UpdateDate = DateTime.Now;
                _Context.SaveChanges();
                return Ok($"Update todo succeed");
            }
        }
    }
}
