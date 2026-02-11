using System.ComponentModel.DataAnnotations;

namespace DotNET.Dtos;

public record class CreateGameDto(
    [property: Required, StringLength(100)] string Name,
    [property: Range(1, int.MaxValue)] int GenreId,
    [property: Range(0, double.MaxValue)] decimal Price,
    [property: Required, NotDefaultDateOnly] DateOnly ReleaseDate
);
