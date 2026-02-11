# GameStore API Notes

## Overview

This repository is a minimal ASP.NET Core API for managing games with SQLite and EF Core. It uses:

- .NET 10 (`net10.0`)
- Minimal APIs
- EF Core + SQLite
- DTOs + mapping extensions
- Validation via `MinimalApis.Extensions`

---

## Quick Start

### Run the API

```bash
dotnet restore
dotnet run
```

Launch profile uses:

- HTTP: `http://localhost:5078`
- HTTPS: `https://localhost:7291`

---

## Project Layout (Actual)

```
DotNET/
 ├── Data/            # DbContext and EF Core config
 ├── Dtos/            # Request/response shapes + validation
 ├── Endpoints/       # Minimal API endpoints
 ├── Entities/        # EF Core entities
 ├── Mapping/         # DTO <-> Entity mapping
 ├── Migrations/      # EF Core migrations
 ├── requests/        # REST Client samples
 ├── GameStore.db     # SQLite database
 ├── Program.cs       # App wiring
 └── appsettings.json # Connection string
```

---

## Core API Endpoints

All endpoints are defined in `Endpoints/GamesEndpoints.cs` under `/games`.

- `GET /` → "Hello World!"
- `GET /games` → list of `GameSummaryDto`
- `GET /games/{id}` → `GameDetailsDto`
- `POST /games` → create game
- `PUT /games/{id}` → update game
- `DELETE /games/{id}` → delete game

---

## Request Examples (from `requests/game.http`)

### Get all games

```http
GET http://localhost:5078/games
```

### Get one game

```http
GET http://localhost:5078/games/1
```

### Create a game

```http
POST http://localhost:5078/games
Content-Type: application/json

{
  "name": "Hockey",
  "genreId": 3,
  "price": 97.99,
  "releaseDate": "2025-01-07"
}
```

### Update a game

```http
PUT http://localhost:5078/games/1
Content-Type: application/json

{
  "name": "Street Fighter II Turbo",
  "genreId": 4,
  "price": 24.99,
  "releaseDate": "1994-02-11"
}
```

### Delete a game

```http
DELETE http://localhost:5078/games/11
```

---

## DTOs and Validation

DTOs live in `Dtos/` and apply DataAnnotations:

- `Name` is required and max length 100
- `GenreId` must be `>= 1`
- `Price` must be `>= 0`
- `ReleaseDate` is required and cannot be default (`NotDefaultDateOnlyAttribute`)

Validation is enabled in `Program.cs` with:

```csharp
builder.Services.AddValidation();
```

and enforced on POST with:

```csharp
.WithParameterValidation()
```

---

## Mapping

Mapping lives in `Mapping/GameMapping.cs` as extension methods:

- `ToGameSummaryDto`
- `ToGameDetailsDto`
- `ToGame`
- `UpdateFrom`

Endpoints project entities to DTOs using `.Select(game => game.ToGameSummaryDto())`.

---

## Data Layer

DbContext: `Data/GameStoreContext.cs`

- `DbSet<Game>` and `DbSet<Genre>`
- Seeds genres on model creation:
  - Fighting
  - Roleplaying
  - Sports
  - Racing
  - Kids and Family

Connection string is in `appsettings.json`:

```json
"ConnectionStrings": {
  "GameStore": "Data Source=GameStore.db"
}
```

---

## EF Core Migrations

Migrations are already created in `Migrations/`:

- `20260211071554_InitialCreate`
- `20260211110000_SeedGenres`

To add a new migration and update the DB:

```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

---

## Useful Files

- `Program.cs` → wiring (DbContext, validation, endpoint mapping)
- `Endpoints/GamesEndpoints.cs` → CRUD endpoints
- `requests/game.http` → ready-to-run REST Client requests
- `GameStore.db` → SQLite DB file (inspect with VS Code SQLite extension)
