using SampleRestAPI.Database;
using Microsoft.EntityFrameworkCore;
using SampleRestAPI.Services;
using SampleRestAPI.Repositories;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Text.Json.Serialization;
using System.Reflection;

namespace SampleRestAPI;
public static class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(System.Text.Json.JsonNamingPolicy.CamelCase));
            })
            .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(Program).Assembly));
            

        builder.Services.AddDbContext<AppDbContext>(options =>
           MySQLDbContextOptionsExtensions.UseMySQL(options, builder.Configuration.GetConnectionString("Main")));
        FulfillDI(builder.Services);
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers(); 


        app.Run();
    }
    private static void FulfillDI(IServiceCollection services)
    {
        services.AddScoped<ToDoService>();
        services.AddScoped<ToDoRepository>();
    }
}