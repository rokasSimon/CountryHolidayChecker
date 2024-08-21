# Country Holiday Checker API

## About

Homework repository to showcase:
- Clean Architecture
- Docker
- CI/CD

## Setup

Before running project you need to generate and trust .NET dev certs to the location configured in the docker compose volume and .env file. The default for this project is:

```sh
dotnet dev-certs https -ep .\certificates\https\countryholidaychecker-api.pfx -p hrkapdo876ABsa0dyasd89dya08isy --trust
```
Afterwards just build the project:
```sh
docker compose --env-file .\.env build
```
Finally, to run the project just supply the .env file to compose:
```sh
docker compose --env-file ./.env up -d
```

## Usage

You can open swagger [here](https://localhost:8081/swagger/index.html).