using Microsoft.EntityFrameworkCore;
using ModelFabryki;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");


app.MapGet("/maszyna", async (ApplicationDbContext db) =>
    await db.Maszyny.ToListAsync());

app.MapGet("/maszyna/{id}", async (int id, ApplicationDbContext db) =>
    await db.Maszyny.FindAsync(id)
        is Maszyna maszyna
        ? Results.Ok(maszyna)
        : Results.NotFound());

app.MapPost("/maszyna", async (Maszyna maszyna, ApplicationDbContext db) =>
{
    db.Maszyny.Add(maszyna);
    await db.SaveChangesAsync();

    return Results.Created($"/maszyna/{maszyna.Id}", maszyna);
});

app.MapPut("/maszyna/{id}", async (int id, Maszyna maszyna, ApplicationDbContext db) =>
{
    var maszynaWBazie = await db.Maszyny.FindAsync(id);

    if (maszynaWBazie is null)
        return Results.NotFound();

    maszynaWBazie.Nazwa = maszyna.Nazwa;
    maszynaWBazie.DataUruchomienia = maszyna.DataUruchomienia;
    maszynaWBazie.HalaId = maszyna.HalaId;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/maszyna/{id}", async (int id, ApplicationDbContext db) =>
{
    if (await db.Maszyny.FindAsync(id) is Maszyna maszyna)
    {
        db.Maszyny.Remove(maszyna);
        await db.SaveChangesAsync();
        return Results.Ok(maszyna);
    }

    return Results.NotFound();
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
