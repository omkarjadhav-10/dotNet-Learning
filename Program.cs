using DotNET.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = new()
{
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
};

app.MapGet("games", () => games);

app.MapGet("/", () => "Hello World!");

app.Run();
