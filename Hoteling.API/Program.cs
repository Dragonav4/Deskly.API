using System.Text.Json;
using System.Text.Json.Serialization;
using Hoteling.API.Extensions;
using Hoteling.Application;
using Hoteling.Infastructure;
using Hoteling.API.Exceptions;
using Hoteling.Infastructure.Data;
using Hoteling.Infastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hoteling.API;
using DotNetEnv;
public static class Program
{
    public static void Main(string[] args)
    {
        Env.Load();
        var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication(builder.Configuration);
        builder.Services.AddAuthorization();

        // Layer Extensions
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication();
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddControllers()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Hoteling System API",
                Version = "v1",
                Description = "API for managing desk reservations in a hoteling system"
            });

            // Enable XML comments for better documentation
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: myAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins("http://localhost:7000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });



        var app = builder.Build();
        app.UseCors(myAllowSpecificOrigins);
        app.UseExceptionHandler();

        // Enable Swagger for OpenAPI generation
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hoteling API v1");
        });
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }

        app.Run();
    }
}
