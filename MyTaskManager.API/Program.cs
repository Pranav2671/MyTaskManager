using Microsoft.EntityFrameworkCore;
using MyTaskManager.API.Data;
using MyTaskManager.API.Repositories;
using System.Text.Json.Serialization;

using static System.Net.WebRequestMethods;

namespace MyTaskManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add DbContext and link it to  connection string
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ITaskRepository, TaskRepository>();


            // Add services to the container.

            //Add Controllers (for API endpoints)

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //Use Swagger only in Development 
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Ensures all communication between client and server is encrypted and secure.
            //HTTP -> HTTPS
            app.UseHttpsRedirection();


            //checks if the user is allowed to access a specific API endpoint.
            app.UseAuthorization();

            //Maps all controllers which we’ll create
            app.MapControllers();

            //Starts the web server,listening for HTTP requests.
            app.Run();
        }
    }
}
