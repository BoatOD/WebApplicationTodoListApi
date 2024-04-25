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
        public TodoController(ILogger<TodoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult List()
        {
            return Ok(todoList);
        }

        [HttpGet("filter")]
        public ActionResult Search([FromQuery] string name)
        {
            return Ok(todoList
                .Where(todo => todo.Title.Contains(name))
                .ToList());
        }

        [HttpPost]
        public ActionResult Add([FromBody] TodoEntryViewModel entry)
        {
            var newTodoEntry = new TodoEntry(entry.Title, entry.Description, entry.DueDate);

            if (todoList.Any(existingTodo => existingTodo.Id == newTodoEntry.Id))
            {
                return Conflict("Duplicated Todo Id");
            }

            todoList.Add(newTodoEntry);
            return Created($"/{newTodoEntry.Id}", entry);
        }

        [HttpDelete("{todoId}")]
        public ActionResult Remove([FromRoute] Guid todoId)
        {
            TodoEntry dataToRemove = todoList.Find(t => t.Id == todoId);
            if (dataToRemove == null)
            {
                return Conflict("This Todo Id doesn't exist.");
            } else
            {
                todoList.Remove(dataToRemove);
                return Ok($"Delete Succeed: {todoId.ToString()}");
            }
        }

        [HttpPut("{todoId}")]
        public ActionResult Replace([FromRoute] Guid todoId, [FromBody] TodoEntryViewModel entry)
        {
            TodoEntry dataToUpdate = todoList.Find(t => t.Id == todoId);

            if (dataToUpdate == null)
            {
                return Conflict("This Todo Id doesn't exist.");
            }
            else
            {
                int index = todoList.IndexOf(dataToUpdate);
                todoList[index].Title = entry.Title;
                todoList[index].Description = entry.Description;
                if (entry.DueDate != null)
                {
                    todoList[index].DueDate = entry.DueDate;
                }
                todoList[index].UpdateDate = DateTime.Now;
                return Ok($"Update todo succeed");
            }
        }
    }
}
