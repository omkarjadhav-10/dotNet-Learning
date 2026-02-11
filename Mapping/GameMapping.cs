using DotNET.Dtos;
using DotNET.Entities;

namespace DotNET.Mapping;

public static class GameMapping
{
    public static GameDto ToGameDto(this Game game) =>
        new(game.Id, game.Name, game.Genre?.Name ?? string.Empty, game.Price, game.ReleaseDate);

    public static GameSummaryDto ToGameSummaryDto(this Game game) =>
        new(game.Id, game.Name, game.Genre?.Name ?? string.Empty);

    public static GameDetailsDto ToGameDetailsDto(this Game game) =>
        new(game.Id, game.Name, game.Genre?.Name ?? string.Empty, game.Price, game.ReleaseDate);

    public static Game ToGame(this CreateGameDto dto, Genre genre) =>
        new()
        {
            Name = dto.Name,
            GenreId = dto.GenreId,
            Genre = genre,
            Price = dto.Price,
            ReleaseDate = dto.ReleaseDate
        };

    public static void UpdateFrom(this Game game, UpdateGameDto dto, Genre genre)
    {
        game.Name = dto.Name;
        game.GenreId = dto.GenreId;
        game.Genre = genre;
        game.Price = dto.Price;
        game.ReleaseDate = dto.ReleaseDate;
    }
}
