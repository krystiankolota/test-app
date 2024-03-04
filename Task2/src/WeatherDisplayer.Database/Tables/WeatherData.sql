CREATE TABLE [dbo].[WeatherData]
(
    [Id] INT PRIMARY KEY IDENTITY,
    [Country] NVARCHAR(100),
    [City] NVARCHAR(100),
    [Temperature] FLOAT,
    [Clouds] INT,
    [WindSpeed] FLOAT,
    [LastUpdate] DATETIME
)
GO

CREATE NONCLUSTERED INDEX [IX_WeatherData_City]
    ON [dbo].[WeatherData]([City])
GO

CREATE NONCLUSTERED INDEX [IX_WeatherData_Country]
    ON [dbo].[WeatherData]([Country])
GO

CREATE NONCLUSTERED INDEX [IX_WeatherData_Temperature]
    ON [dbo].[WeatherData]([Temperature])
INCLUDE (Country, WindSpeed);
GO