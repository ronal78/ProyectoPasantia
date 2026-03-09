using Backend.Api;
using Backend.Data.Models;
using Backend.Dependencies;
using Backend.Service;
using Microsoft.Extensions.Options;

namespace PasantiaTI1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddNewtonsoftJson();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
            
            builder.Services.AddScoped(opt =>
            {
                var settings = opt.GetRequiredService<IOptions<MongoSettings>>().Value;
                return new MongoDbContext(settings.ConnectionString, settings.DatabaseName);
            });


            builder.Services.AddScoped<ISeriesDependencies, SeriesDependencies>();
            builder.Services.AddScoped<SeriesService>();
            

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
                
            // Configurate the HTTP request pipeline

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }

}