# test-app

## General

I didn't create tests for everything. Added some sample tests to save some time. I applied clean architecture with some form of DDD. I didn't use full DDD with valueObjects, aggregates and so on since that approach is debateable.

## TEST1

Didn't provide docker support here since it's simple app.

Implemented a form of cqrs with mediatR

It's saving metadata in table storage and payload in blob storage

### Requirements:
- azurite or storage in azure
- settings
```
"LogRepositorySettings:ConnectionString": "UseDevelopmentStorage=true"
"PayloadStorageSettings:ConnectionString": "UseDevelopmentStorage=true"
```

By default it's using azurite

Entry point: ```RandomDataFetcher.FunctionApp```
- .NET6 in-process Azure Function

Don't forget to create ```local.settings.json``` from ```local.settings.json.example```

## TEST2

Provided docker support here

```docker-compose.yml``` file is in src/ directory

If you are using docker then the only thing needed is to fill ```WeatherApiClientSettings__ApiKey: ""``` inside docker compose file.

There is a need to create an account and generate api key here: ```https://www.weatherapi.com/```

Docker compose will spin-up backend-api, database and frontent server

Default Endpoints
```
DB: localhost,1433
Backend: localhost:8080
Frontend: localhost:3000
```

### After First Startup

- Publish WeatherDisplayer.Database.sqlproj on table initialized by docker
- It will create table schema, indexes and stored procedures

### Usage

- Frontend is calling for new data every minute
- Backend is calling for fresh data every minute (BackGround Service)
- Trend graph is shown after a click on a dot on existing wind and temperature graphs