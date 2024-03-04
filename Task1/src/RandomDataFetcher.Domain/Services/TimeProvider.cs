using RandomDataFetcher.Domain.Common.Interfaces;
namespace RandomDataFetcher.Domain.Services;

public sealed class TimeProvider : ITimeProvider
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;
}
