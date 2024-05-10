//using Microsoft.VisualBasic;
//using System.ComponentModel.DataAnnotations;

//namespace WebApplicationTodoList.Models;

//public class TodoTags
//{
//    [Key]
//    public Guid Id { get; set; }
//    [Required]
//    [Length(1,100)]
//    [RegularExpression("^\\w+$", MatchTimeoutInMilliseconds = 1000)]
//    public string Name { get; set; } = string.Empty;

//    public List<TodoEntry> TaggedEntries { get; } = [];
//}
