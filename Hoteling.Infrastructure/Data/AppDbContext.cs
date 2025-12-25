using Hoteling.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hoteling.Infastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Desk> Desks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Desk)
            .WithMany()
            .HasForeignKey(r => r.DeskId);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Desk>().HasData( //Todo make separate file for seed data in SQL folder
            new Desk { Id = Guid.Parse("d3e4f5a6-b7c8-4d0e-8f2e-3c4d5e6f7a8b"), DeskNumber = "A-001", Floor = 1, HasDualMonitor = true, IsStandingDesk = false },
            new Desk { Id = Guid.Parse("a1b2c3d4-e5f6-4080-9000-010203040506"), DeskNumber = "B-005", Floor = 2, HasDualMonitor = false, IsStandingDesk = true }
        );
    }
}