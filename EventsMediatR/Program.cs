using MediatR;
using EventsBackgroundService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddHostedService<TickerBackgroundService>();

var app = builder.Build();

app.Run();
