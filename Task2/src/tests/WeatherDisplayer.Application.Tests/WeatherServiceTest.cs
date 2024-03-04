using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using WeatherDisplayer.Application.Common.Interfaces;
using WeatherDisplayer.Application.Common.Settings;
using WeatherDisplayer.Application.Service;
using WeatherDisplayer.Domain.Interfaces;
using WeatherDisplayer.Domain.Models;
namespace WeatherDisplayer.Application.Tests;

public class WeatherServiceTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IWeatherDataRepository> _weatherDataRepositoryMock;
    private readonly Mock<IOptions<WeatherApiClientSettings>> _weatherApiSettingsMock;
    private readonly Mock<ITimeProvider> _timeProviderMock;

    private readonly IWeatherService _sut;

    public WeatherServiceTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _weatherDataRepositoryMock = _fixture.Freeze<Mock<IWeatherDataRepository>>();
        _weatherApiSettingsMock = _fixture.Freeze<Mock<IOptions<WeatherApiClientSettings>>>();
        _timeProviderMock = _fixture.Freeze<Mock<ITimeProvider>>();

        _timeProviderMock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.Parse("2024-01-01"));

        _sut = _fixture.Create<WeatherService>();
    }

    [Fact]
    public async Task GetAsync_PassCityName_RetrieveData()
    {
        // Arrange
        var cityName = _fixture.Create<string>();
        var weatherData = _fixture
            .CreateMany<WeatherData>()
            .ToList();

        _weatherDataRepositoryMock.Setup(r => r.GetAsync(
                It.IsAny<Func<WeatherData, bool>>(),
                It.IsAny<Func<IQueryable<WeatherData>, IOrderedQueryable<WeatherData>>>(),
                true))
            .ReturnsAsync(weatherData);

        // Act
        var result = await _sut.GetAsync(cityName);

        // Assert
        result.Should().BeEquivalentTo(weatherData);
    }

    [Fact]
    public async Task GetAsync_ReturnsWeatherDataForCity()
    {
        // Arrange
        var cityName = _fixture.Create<string>();
        var expectedData = _fixture
            .CreateMany<WeatherData>()
            .ToList();

        _weatherDataRepositoryMock
            .Setup(repo => repo.GetAsync(
                It.IsAny<Func<WeatherData, bool>>(),
                It.IsAny<Func<IQueryable<WeatherData>,
                    IOrderedQueryable<WeatherData>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(expectedData);

        // Act
        var result = await _sut.GetAsync(cityName);

        // Assert
        result.Should().BeEquivalentTo(expectedData);
    }

    [Fact]
    public async Task GetTrends_ReturnsWeatherDataForLastTwoHours()
    {
        // Arrange
        var cityName = _fixture.Create<string>();
        var expectedData = _fixture
            .Build<WeatherData>()
            .With(x => x.LastUpdate, DateTime.Parse("2024-01-01"))
            .CreateMany()
            .ToList();

        _weatherDataRepositoryMock.Setup(repo => repo.GetAsync(
                It.IsAny<Func<WeatherData, bool>>(),
                It.IsAny<Func<IQueryable<WeatherData>,
                    IOrderedQueryable<WeatherData>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(expectedData);

        // Act
        var result = await _sut.GetTrends(cityName);

        // Assert
        result.Should().BeEquivalentTo(expectedData);
    }
}
