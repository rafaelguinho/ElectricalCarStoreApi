using ElectricalCarStoreApi.Context;
using ElectricalCarStoreApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CarDb>(options =>
{
    var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    options.UseSqlite($"Data Source={Path.Join(path, "Cars.db")}");
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetService<CarDb>();
db?.Database.MigrateAsync();

app.MapGet("/cars", async (CarDb db) =>
    await db.Cars.ToListAsync());

//app.MapGet("/car/complete", async (CarDb db) =>
//    await db.Cars.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/cars/{id:guid}", async (Guid id, CarDb db) =>
    await db.Cars.FindAsync(id)
        is Car car
            ? Results.Ok(car)
            : Results.NotFound());

app.MapPost("/cars", async (Car Car, CarDb db) =>
{
    db.Cars.Add(Car);
    await db.SaveChangesAsync();

    return Results.Created($"/cars/{Car.Id}", Car);
});

app.MapPut("/cars/{id}", async (Guid id, Car inputCar, CarDb db) =>
{
    var car = await db.Cars.FindAsync(id);

    if (car is null) return Results.NotFound();

    car.Description = inputCar.Description;
    car.ImageUrl = inputCar.ImageUrl;
    car.City = inputCar.City;
    car.ManufactureYear = inputCar.ManufactureYear;
    car.KilometersDriven = inputCar.KilometersDriven;
    car.Brand = inputCar.Brand;
    car.Model = inputCar.Model;
    car.ModelYear = inputCar.ModelYear;
        
    car.Price = inputCar.Price;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/cars/{id}", async (Guid id, CarDb db) =>
{
    if (await db.Cars.FindAsync(id) is Car Car)
    {
        db.Cars.Remove(Car);
        await db.SaveChangesAsync();
        return Results.Ok(Car);
    }

    return Results.NotFound();
});

app.Run();