using LightningCars.Api.Models;
using Microsoft.EntityFrameworkCore;
using RentalCar.Api.Models;

namespace RentalCar.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
