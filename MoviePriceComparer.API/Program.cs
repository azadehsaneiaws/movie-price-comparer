using MoviePriceComparer.API.Domain.Interfaces;
using MoviePriceComparer.API.Infrastructure.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
builder.Services.AddHttpClient("Cinemaworld", client =>
{
    client.BaseAddress = new Uri("https://webjetapitest.azurewebsites.net/api/cinemaworld/");
    client.DefaultRequestHeaders.Add("x-access-token", builder.Configuration["ApiToken"]);
});

builder.Services.AddHttpClient("Filmworld", client =>
{
    client.BaseAddress = new Uri("https://webjetapitest.azurewebsites.net/api/filmworld/");
    client.DefaultRequestHeaders.Add("x-access-token", builder.Configuration["ApiToken"]);
});

builder.Services.AddScoped<IMovieProvider, CinemaWorldProvider>();
builder.Services.AddScoped<IMovieProvider, FilmworldProvider>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddMemoryCache();


