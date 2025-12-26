using Hoteling.API.Options;
using Hoteling.Application.Services;
using Hoteling.Domain.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace Hoteling.API.Extensions;

public static class AddAuthenticationExtension
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var authOptions = config.GetSection("AuthOptions:Google").Get<AuthOptions>()
                          ?? throw new InvalidOperationException("AuthOptions section is missing in appsettings.json");
        services.Configure<AuthOptions>(config.GetSection("AuthOptions"));
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddCookie()
            .AddGoogle(options =>
            {
                options.ClientId = authOptions.ClientId;
                options.ClientSecret = authOptions.ClientSecret;

                options.Events.OnCreatingTicket = async context =>
                {
                    var email = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                    var name = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

                    if (string.IsNullOrEmpty(email)) return;

                    var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                    var user = await userService.GetUserByEmail(email);

                    if (user == null)
                    {
                        var newUser = new Domain.Entities.User
                        {
                            Id = Guid.NewGuid(),
                            Email = email,
                            UserName = name ?? email,
                            Role = UserRole.Guest
                        };
                        await userService.CreateAsync(newUser);
                    }
                };
            });
        return services;
    }
}
