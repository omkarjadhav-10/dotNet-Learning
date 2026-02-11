

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

        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.ToGameSummaryDto())
                .ToListAsync());

        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(g => g.Id == id);
            return game is not null
                ? Results.Ok(game.ToGameDetailsDto())
                : Results.NotFound();
        }).WithName("GetGame");

        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            var genre = await dbContext.Genres.FindAsync(newGame.GenreId);
            if (genre is null)
            {
                return Results.BadRequest($"Invalid genre id: {newGame.GenreId}");
            }

            Game game = newGame.ToGame(genre);
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game.ToGameDetailsDto());
        })
        .WithParameterValidation();

        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);

            if (game is null)
            {
                return Results.NotFound();
            }

            var genre = await dbContext.Genres.FindAsync(updatedGame.GenreId);
            if (genre is null)
            {
                return Results.BadRequest($"Invalid genre id: {updatedGame.GenreId}");
            }

            game.UpdateFrom(updatedGame, genre);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);

            if (game is null)
            {
                return Results.NotFound();
            }

            dbContext.Games.Remove(game);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        app.MapGet("/", () => "Hello World!");

        return app;
    }
}
