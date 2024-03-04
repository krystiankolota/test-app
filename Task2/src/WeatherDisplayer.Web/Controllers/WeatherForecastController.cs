using Microsoft.AspNetCore.Mvc;
using WeatherDisplayer.Application.Common.Interfaces;
using WeatherDisplayer.Contracts;
using WeatherDisplayer.Domain.Models;
namespace WeatherDisplayer.Web.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherDataController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherDataController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        [Route("city/{cityName}")]
        public async Task<IActionResult> GetWeather(
            [FromRoute] string cityName)
        {
            var data = await _weatherService.GetAsync(cityName);

            if (data == null)
            {
                return BadRequest();
            }

            return Ok(Map(data));
        }

        [HttpGet]
        [Route("maxwindspeed/country/{countryName}")]
        public async Task<IActionResult> GetMaxWindSpeedByCountry([FromRoute] string countryName)
        {
            var data = await _weatherService.GetMaxWindSpeedByCountryAsync(countryName);
            return Ok(data);
        }

        [HttpGet]
        [Route("temperature")]
        public async Task<IActionResult> GetWeatherDataByTemperatureAsync([FromQuery] float temperature)
        {
            var data = await _weatherService.GetWeatherDataByTemperatureAsync(temperature);
            return Ok(data);
        }

        [HttpGet("trend/{city}")]
        public async Task<IActionResult> GetTrends(string city)
        {
            var data = await _weatherService.GetTrends(city);
            return Ok(Map(data));
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var data = await _weatherService.GetSummaryAsync();
            return Ok(data);
        }

        private static IList<WeatherDataDto> Map(IEnumerable<WeatherData> weatherData)
        {
            return weatherData
                .Select(d => new WeatherDataDto(d.Country, d.City, d.Clouds, d.Temperature, d.WindSpeed, d.LastUpdate))
                .ToList();
        }
    }
}
