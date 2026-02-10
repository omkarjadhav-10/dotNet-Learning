using DotNET.Dtos;
using DotNET.Endpoints;
using Microsoft.VisualBasic;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGamesEndpoints();
app.Run();
