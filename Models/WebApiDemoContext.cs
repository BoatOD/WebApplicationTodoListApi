using Microsoft.EntityFrameworkCore;

namespace WebApplicationTodoList.Models
{
    public class WebApiDemoContext : DbContext
    {
        public WebApiDemoContext(DbContextOptions<WebApiDemoContext> options) : base(options) 
        { 

        }

        public DbSet<TodoEntry> TodoEntries { get; set; } = null!;
        //public DbSet<TodoTags> Tags { get; set; } = null!;

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<TodoEntry>()
        //        .HasMany(e => e.Tags)
        //        .WithMany(e => e.TaggedEntries);
        //}
    }
}
