using MediatR;
using RandomDataFetcher.Domain.Models;
namespace RandomDataFetcher.Application.Services.RandomLogs.Queries.ListLogsQuery;

public record ListLogsQuery(DateTime From, DateTime To) : IRequest<IList<LogEntry>>;
