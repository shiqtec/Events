using EventsBackgroundService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<TickerBackgroundService>();

var app = builder.Build();

app.Run();
