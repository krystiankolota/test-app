using MediatR;
namespace RandomDataFetcher.Application.Services.RandomLogs.Queries.GetLogQuery;

public record GetLogQuery(Guid Id) : IRequest<string>;
