using MediatR;
using EventsBackgroundService;
using EventsMediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddTransient<TransientGUIDService>();
builder.Services.AddHostedService<TickerBackgroundService>();

var app = builder.Build();

app.Run();
