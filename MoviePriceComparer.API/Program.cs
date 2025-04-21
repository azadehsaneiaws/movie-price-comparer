using MoviePriceComparer.API.Domain.Interfaces;
using MoviePriceComparer.API.Infrastructure.Providers;

var builder = WebApplication.CreateBuilder(args);

// Register services 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClients for the two providers
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

// DI registrations
builder.Services.AddScoped<IMovieProvider, CinemaWorldProvider>();
builder.Services.AddScoped<IMovieProvider, FilmworldProvider>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddControllers();
builder.Services.AddMemoryCache();

//  Now build the app
var app = builder.Build();
app.MapControllers();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
