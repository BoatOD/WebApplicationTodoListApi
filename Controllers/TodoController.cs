using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using WebApplicationTodoList.ViewModels;
using WebApplicationTodoList.Models;
using System.Linq;
using WebApplicationTodoList.Services;

namespace WebApplicationTodoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private static List<TodoEntry> todoList = new();
        private readonly ILogger<TodoController> _logger;

        private readonly WebApiDemoContext _Context;
        private readonly TodoService _todoService;

        public TodoController(ILogger<TodoController> logger, TodoService todoService, WebApiDemoContext context)
        {
            _logger = logger;
            _Context = context;
            _todoService = todoService;
        }

        [HttpGet]
        public ActionResult List()
        {
            return Ok(_todoService.GetAll());
        }

        [HttpGet("filter")]
        public ActionResult Search([FromQuery] string name)
        {
            return Ok(_todoService.FilterByName(name));
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] TodoEntryViewModel entry)
        {
            var newTodoEntry = new TodoEntry(entry.Title, entry.Description, entry.DueDate, entry.Status);
            if (!await _todoService.AddTodo(newTodoEntry))
            {
                return Conflict("Duplicate Todo.");
            }
            return Created($"/{newTodoEntry.Id}", entry);
        }

        [HttpDelete("{todoId}")]
        public async Task<ActionResult> Remove([FromRoute] Guid todoId)
        {
            if (!await _todoService.RemoveTodo(todoId))
            {
                return Conflict("This Todo Id doesn't exist.");
            } else
            {
                return Ok($"Delete Succeed: {todoId.ToString()}");
            }
        }

        [HttpPut("{todoId}")]
        public async Task<ActionResult> Replace([FromRoute] Guid todoId, [FromBody] TodoEntryViewModel entry)
        {
            if (await _todoService.UpdateTodo(todoId, entry))
            {
                return Ok($"Update Succeed");
            }
            else
            {
                return Conflict("This Todo Id doesn't exist.");
            }
        }

        [HttpPut("status/{todoId}")]
        public async Task<ActionResult> ReplaceStatus([FromRoute] Guid todoId, [FromBody] string status)
        {
            if (await _todoService.UpdateStatusTodo(todoId, status))
            {
                return Ok($"Update Status Succeed");
            }
            else
            {
                return Conflict("This Todo Id doesn't exist.");
            }
        }
    }
}
