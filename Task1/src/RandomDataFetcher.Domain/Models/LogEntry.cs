namespace RandomDataFetcher.Domain.Models;

public class LogEntry
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public bool Success { get; set; }
    public string PayloadUri { get; set; } = null!;
}
