

using System;
using DotNET.Dtos;

namespace DotNET.Endpoints;

public static class GamesEndpoints
{

    private static readonly List<Dtos.GameDto> games =
[
    new(
       1,
        "Street Fighter II",
        "Fighting",
        19.99m,
        new DateOnly(1992, 7, 15)
    ),
    new(
        2,
        "Final Fantasy XIV",
        "Roleplaying",
        59.99m,
        new DateOnly(2010, 9, 30)
    ),
    new(
        3,
        "FIFA 23",
        "Sports",
        69.99m,
        new DateOnly(2022, 9, 27)
    ),
    new(
        4,
        "The Legend of Zelda: Breath of the Wild",
        "Adventure",
        59.99m,
        new DateOnly(2017, 3, 3)
    ),
    new(
        5,
        "Minecraft",
        "Sandbox",
        26.95m,
        new DateOnly(2011, 11, 18)
    )
];

    public static WebApplication MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        group.MapGet("/", () => games);

        group.MapGet("/{id}", (int id) =>
        {
            var game = games.FirstOrDefault(g => g.Id == id);
            return game is not null
                ? Results.Ok(game)
                : Results.NotFound();
        }).WithName("GetGame");

        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );
            games.Add(game);
            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
        })
        .WithParameterValidation();

        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );
            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games.RemoveAt(index);
            return Results.NoContent();
        });

        app.MapGet("/", () => "Hello World!");

        return app;
    }
}
