using Hoteling.Application.Interfaces;
using Hoteling.Application.Interfaces.IService;
using Hoteling.Application.Services;
using Hoteling.Application.Views.Desk;
using Hoteling.Application.Views.Reservation;
using Hoteling.Application.Views.User;
using Hoteling.Application.ViewsMapper;
using Hoteling.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Hoteling.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDeskService, DeskService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped(typeof(IService<>), typeof(CrudService<>));
        services.AddScoped<IService<Reservation>, ReservationService>();
        // Mappers
        services.AddScoped<ICrudMapper<Desk, DeskCreateView, DeskView>, DeskMapper>();
        services.AddScoped<ICrudMapper<User, UserCreateView, UserView>, UserMapper>();
        services.AddScoped<ICrudMapper<Reservation, ReservationCreateView, ReservationView>, ReservationMapper>();

        return services;
    }
}
