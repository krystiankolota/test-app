CREATE PROCEDURE GetMaxWindSpeedByCountry
    @CountryName NVARCHAR(100)
AS
BEGIN
    SELECT MAX(WindSpeed) as MaxWindSpeed, Country
    FROM WeatherData
    WHERE Country = @CountryName
    GROUP BY Country;
END