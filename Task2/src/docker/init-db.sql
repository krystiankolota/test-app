USE [master]
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'weather-data-db')
BEGIN
    CREATE DATABASE [weather-data-db];
END;
GO

USE weather-data-db;