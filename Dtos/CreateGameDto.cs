using System.ComponentModel.DataAnnotations;

namespace DotNET.Dtos;

public record class CreateGameDto(
    [property: Required, StringLength(100)] string? Name,
    [property: Required, StringLength(50)] string? Genre,
    [property: Range(0, double.MaxValue)] decimal Price,
    [property: Required, NotDefaultDateOnly] DateOnly ReleaseDate
);
