using AutoMapper;
using AutoMapper.QueryableExtensions;
using ElectricalCarStoreApi.Context;
using ElectricalCarStoreApi.Mapper;
using ElectricalCarStoreApi.Models;
using ElectricalCarStoreApi.ViewModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CarDb>(options =>
{
    var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    options.UseSqlite($"Data Source={Path.Join(path, "Cars.db")}");
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


MapperConfiguration mappingConfig = new(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});

IMapper mapper = mappingConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

builder.Services.AddAutoMapper(typeof(Program));

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://127.0.0.1:5173");
                      });
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetService<CarDb>();
db?.Database.MigrateAsync();

app.MapGet("/cars", async (CarDb db, IMapper mapper) =>
    await db.Cars.ProjectTo<CarListViewModel>(mapper.ConfigurationProvider).ToListAsync());

app.MapGet("/cars/ids", async (CarDb db) =>
    await db.Cars.Select(c => c.Id).ToListAsync());

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

    car.PlateLastNumber = inputCar.PlateLastNumber;
    car.TransmissionType = inputCar.TransmissionType;
    car.Color = inputCar.Color;

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



app.UseCors(MyAllowSpecificOrigins);

app.Run();