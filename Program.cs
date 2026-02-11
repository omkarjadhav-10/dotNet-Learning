using DotNET.Data;
using DotNET.Dtos;
using DotNET.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();
var connectionString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddDbContext<GameStoreContext>(options => options.UseSqlite(connectionString));
var app = builder.Build();

app.MapGamesEndpoints();
app.Run();
