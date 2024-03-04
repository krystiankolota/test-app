CREATE PROCEDURE GetWeatherDataByTemperature
    @Temperature FLOAT
AS
BEGIN
    SELECT MIN(Temperature) as MinTemperature, MAX(WindSpeed) as MaxWindSpeed, Country
    FROM WeatherData
    WHERE Temperature < @Temperature
    GROUP BY Country;
END