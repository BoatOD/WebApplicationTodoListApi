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
            var dataToRemove = todoList.Where(t => t.Id.ToString().Contains(todoId.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (dataToRemove.Count() <= 0)
            {
                return Conflict("This Todo Id doesn't exist.");
            } else
            {
                todoList.Remove(dataToRemove[0]);
                return Ok($"Delete Succeed: {todoId.ToString()}");
            }
        }

        [HttpPut("{todoId}")]
        public ActionResult Replace([FromRoute] Guid todoId, [FromBody] TodoEntryViewModel entry)
        {
            throw new NotImplementedException();
        }
    }
}
