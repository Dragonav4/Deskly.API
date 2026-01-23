using System.Text.Json;
using System.Text.Json.Serialization;
using Hoteling.API.Extensions;
using Hoteling.Application;
using Hoteling.Infrastructure;
using Hoteling.API.Exceptions;
using Hoteling.Infrastructure.Data;
using Hoteling.Infrastructure.Extensions;
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
        builder.Services.AddSwaggerGen();
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
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
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
