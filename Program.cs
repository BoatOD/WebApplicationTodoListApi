
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using WebApplicationTodoList.Models;
using WebApplicationTodoList.Services;

namespace WebApplicationTodoList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            // Add services to the container.

            //builder.Services.AddAuthentication("BasicAuthentication")
            //    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            builder.Services.AddScoped<TodoService>();

            builder.Services.AddControllers();
            builder.Services.AddDbContext<WebApiDemoContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                }
                );
            });
                  

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            //app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
