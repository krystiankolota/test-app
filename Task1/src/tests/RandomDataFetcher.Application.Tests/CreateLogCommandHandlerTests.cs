using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using RandomDataFetcher.Application.Common.Interfaces.Infrastructure;
using RandomDataFetcher.Application.Services.RandomLogs.Commands.CreateLogCommand;
using RandomDataFetcher.Domain.Models;
namespace RandomDataFetcher.Application.Tests;

public class CreateLogCommandHandlerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IRandomLogsRepository> _mockRandomLogsRepository;
    private readonly Mock<IRandomLogsPayloadStorage> _mockRandomLogsPayloadStorage;
    private readonly CreateLogCommandHandler _handler;

    public CreateLogCommandHandlerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _mockRandomLogsRepository = _fixture.Freeze<Mock<IRandomLogsRepository>>();
        _mockRandomLogsPayloadStorage = _fixture.Freeze<Mock<IRandomLogsPayloadStorage>>();
        _handler = _fixture.Create<CreateLogCommandHandler>();
    }

    [Fact]
    public async Task Handle_ShouldStorePayloadAndCreateLogEntry()
    {
        // Arrange
        var command = _fixture.Create<CreateLogCommand>();
        var expectedLogEntry = _fixture.Create<LogEntry>();
        var (id, blobUrl) = _fixture.Create<(Guid Id, string BlobUrl)>();

        _mockRandomLogsPayloadStorage
            .Setup(x => x.StoreAsync(command.Payload))
            .ReturnsAsync((id, blobUrl));

        _mockRandomLogsRepository
            .Setup(x => x.UpsertAsync(command.IsSuccess, blobUrl, id, command.TimeStamp))
            .ReturnsAsync(expectedLogEntry);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedLogEntry);
    }

    [Fact]
    public async Task Handle_ShouldCallStoreAsyncOnce()
    {
        // Arrange
        var command = _fixture.Create<CreateLogCommand>();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRandomLogsPayloadStorage
            .Verify(x => x.StoreAsync(command.Payload),
                Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCallUpsertAsyncOnce()
    {
        // Arrange
        var command = _fixture.Create<CreateLogCommand>();
        var storeResult = _fixture.Create<(Guid Id, string BlobUrl)>();

        _mockRandomLogsPayloadStorage
            .Setup(x => x.StoreAsync(command.Payload))
            .ReturnsAsync(storeResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRandomLogsRepository
            .Verify(x => x.UpsertAsync(
                    command.IsSuccess,
                    storeResult.BlobUrl,
                    storeResult.Id,
                    command.TimeStamp),
                Times.Once);
    }
}
