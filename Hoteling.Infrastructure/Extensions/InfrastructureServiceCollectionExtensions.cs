using Hoteling.Application.Interfaces.IRepository;
using Hoteling.Domain.Entities;
using Hoteling.Infrastructure.Data;
using Hoteling.Infrastructure.Options;
using Hoteling.Infrastructure.Repositories;
using Hoteling.Infrastructure.Repositories.Desks;
using Hoteling.Infrastructure.Repositories.Reservations;
using Hoteling.Infrastructure.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hoteling.Infrastructure.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Options
        services.Configure<DatabaseOptions>(configuration.GetSection("Database"));

        // Database
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            
            if (dbOptions.UsePostgres)
            {
                options.UseNpgsql(dbOptions.PostgresConnection);
            }
            else
            {
                options.UseSqlite(dbOptions.ConnectionString);
            }
        });

        // Repositories
        services.AddScoped(typeof(ICrudRepository<>), typeof(CrudRepository<>));

        services.AddScoped<ICrudRepository<Reservation>, ReservationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDeskRepository, DeskRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        return services;
    }
}
