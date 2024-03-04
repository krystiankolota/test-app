using MediatR;
using RandomDataFetcher.Domain.Models;
namespace RandomDataFetcher.Application.Services.RandomLogs.Commands.CreateLogCommand;

public record CreateLogCommand(string Payload, bool IsSuccess, DateTime TimeStamp)
    : IRequest<LogEntry>;
