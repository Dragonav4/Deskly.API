using Hoteling.API.Extensions;
using Hoteling.Application;
using Hoteling.Infastructure;
using Hoteling.Infastructure.Data;
using Hoteling.Infastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hoteling.API;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication(builder.Configuration);
        builder.Services.AddAuthorization();

        // Layer Extensions
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

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
