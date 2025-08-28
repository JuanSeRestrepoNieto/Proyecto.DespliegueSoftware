using Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching", "PruebaConglicto"
};

var forecasts = new List<WeatherForecast>();

app.MapPost("/weatherforecast", (WeatherForecast input) =>
{
     forecasts.Add(input);
     return Results.Created($"/weatherforecast/{input.Date:yyyy-MM-dd}", input);
});

app.MapGet("/weatherforecast", () =>
{
     if (forecasts.Count > 0)
          return Results.Ok(forecasts);

     var summaries = new[]
     {
        "Freezing","Bracing","Chilly","Cool","Mild","Warm",
        "Balmy","Hot","Sweltering","Scorching"
    };

     var forecast = Enumerable.Range(1, 5).Select(index =>
         new WeatherForecast
         (
             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
             Random.Shared.Next(-20, 55),
             summaries[Random.Shared.Next(summaries.Length)]
         ))
         .ToArray();

     return Results.Ok(forecast);
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapPut("/weatherforecast/{date}", (string date, WeatherForecast updated) =>
{
    if (!DateOnly.TryParse(date, out var parsedDate))
    {
        return Results.BadRequest("Formato de fecha inválido. Usa yyyy-MM-dd.");
    }

    var existing = forecasts.FirstOrDefault(f => f.Date == parsedDate);
    if (existing is null)
    {
        return Results.NotFound($"No se encontró el pronóstico con fecha {date}.");
    }

    // Como usaste `record`, debes reemplazarlo en la lista (son inmutables)
    forecasts.Remove(existing);
    forecasts.Add(updated);

    return Results.Ok(updated);
});


app.MapGet("helloWorld", () => {
    HomeController homeController= new HomeController();
    return homeController.HelloWorld();
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
