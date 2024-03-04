using Microsoft.EntityFrameworkCore;
using WeatherDisplayer.Contracts;
using WeatherDisplayer.Domain.Models;
namespace WeatherDisplayer.Infrastructure.Database;

public sealed class WeatherDataContext : DbContext
{
    public WeatherDataContext(DbContextOptions<WeatherDataContext> options)
        : base(options)
    {
        Database.SetCommandTimeout(TimeSpan.FromMinutes(15));
    }

    public DbSet<WeatherData> WeatherData { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(WeatherDataContext).Assembly);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WeatherByTemperatureInfo>()
            .HasNoKey()
            .ToView(nameof(WeatherByTemperatureInfo));

        modelBuilder.Entity<MaxWindSpeedInfo>()
            .HasNoKey()
            .ToView(nameof(MaxWindSpeedInfo));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
