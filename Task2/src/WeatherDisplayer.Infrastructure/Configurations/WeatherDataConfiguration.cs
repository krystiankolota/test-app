using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherDisplayer.Domain.Models;
namespace WeatherDisplayer.Infrastructure.Configurations;

public sealed class WeatherDataConfiguration : IEntityTypeConfiguration<WeatherData>
{
    public void Configure(EntityTypeBuilder<WeatherData> builder)
    {
        builder.ToTable(nameof(WeatherData));

        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.City)
            .HasMaxLength(100);

        builder
            .Property(e => e.Country)
            .HasMaxLength(100);

        builder
            .Property(e => e.LastUpdate);

        builder
            .Property(e => e.Temperature);

        builder
            .Property(e => e.WindSpeed);
    }
}
