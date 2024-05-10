using System.ComponentModel.DataAnnotations;
using WebApplicationTodoList.Models;

namespace WebApplicationTodoList.ViewModels
{
    public class TodoEntryViewModel
    {
        [StringLength(100, MinimumLength = 1)]
        [RegularExpression("[\\w\\s]+")]
        public string Title { get; set; } = string.Empty;


        [RegularExpression("[\\w\\s]+")]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        //public string[] Tags { get; set; }
    }
}
