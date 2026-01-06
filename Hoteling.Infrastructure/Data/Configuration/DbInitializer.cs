using Hoteling.Domain.Entities;
using Hoteling.Domain.Enums;

namespace Hoteling.Infastructure.Data.Configuration;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            var admin = new User
            {
                Id = Guid.Parse("3AE864DD-1AE5-4919-9F9C-E7486C9AE41C"),
                UserName = "Danyil Danilian",
                Email = "s31722@pjwstk.edu.pl",
                Role = UserRole.Admin
            };
            context.Users.Add(admin);
        }

        if (!context.Desks.Any())
        {
            var desk1 = new Desk
            {
                Id = Guid.Parse("d3e4f5a6-b7c8-4d0e-8f2e-3c4d5e6f7a8b"),
                DeskNumber = "A-001",
                Floor = 1,
                HasDualMonitor = true,
                IsStandingDesk = false
            };
            var desk2 = new Desk
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-4080-9000-010203040506"),
                DeskNumber = "B-005",
                Floor = 2,
                HasDualMonitor = false,
                IsStandingDesk = true
            };
            context.Desks.AddRange(desk1, desk2);
        }

        if (!context.Reservations.Any() && context.Users.Any() && context.Desks.Any())
        {
            var adminId = Guid.Parse("3AE864DD-1AE5-4919-9F9C-E7486C9AE41C");
            var desk1Id = Guid.Parse("d3e4f5a6-b7c8-4d0e-8f2e-3c4d5e6f7a8b");

            context.Reservations.Add(new Reservation
            {
                Id = Guid.NewGuid(),
                UserId = adminId,
                DeskId = desk1Id,
                ReservationDate = DateTime.Today.AddDays(1),
                CreatedAt = DateTime.UtcNow
            });
        }

        context.SaveChanges();
    }
}
