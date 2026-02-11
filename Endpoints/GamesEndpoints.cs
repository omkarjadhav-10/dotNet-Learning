

using System;
using DotNET.Data;
using DotNET.Dtos;
using DotNET.Entities;
using DotNET.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DotNET.Endpoints;

public static class GamesEndpoints
{

    public static WebApplication MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        group.MapGet("/", (GameStoreContext dbContext) =>
            dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.ToGameSummaryDto())
                .ToList());

        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
        {
            var game = dbContext.Games
                .Include(g => g.Genre)
                .FirstOrDefault(g => g.Id == id);
            return game is not null
                ? Results.Ok(game.ToGameDetailsDto())
                : Results.NotFound();
        }).WithName("GetGame");

        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            var genre = dbContext.Genres.Find(newGame.GenreId);
            if (genre is null)
            {
                return Results.BadRequest($"Invalid genre id: {newGame.GenreId}");
            }

            Game game = newGame.ToGame(genre);
            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game.ToGameDetailsDto());
        })
        .WithParameterValidation();

        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var game = dbContext.Games.Find(id);

            if (game is null)
            {
                return Results.NotFound();
            }

            var genre = dbContext.Genres.Find(updatedGame.GenreId);
            if (genre is null)
            {
                return Results.BadRequest($"Invalid genre id: {updatedGame.GenreId}");
            }

            game.UpdateFrom(updatedGame, genre);
            dbContext.SaveChanges();
            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) =>
        {
            var game = dbContext.Games.Find(id);

            if (game is null)
            {
                return Results.NotFound();
            }

            dbContext.Games.Remove(game);
            dbContext.SaveChanges();
            return Results.NoContent();
        });

        app.MapGet("/", () => "Hello World!");

        return app;
    }
}
