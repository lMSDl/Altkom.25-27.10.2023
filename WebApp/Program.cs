using Microsoft.AspNetCore.Authorization;
using Models;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSingleton<List<int>>(x => new List<int>
{
    2,5,843,1,21
});
builder.Services.AddSingleton<IShoppingListsService, ShoppingListsService>();
builder.Services.AddTransient<EntityFaker<ShoppingList>, ShoppingListFaker>();


builder.Services.AddSingleton<IPeopleService, PeopleService>();
builder.Services.AddTransient<EntityFaker<Person>, PersonFaker>();

builder.Services.AddSingleton<IProductsService, ProductsService>();
builder.Services.AddTransient<EntityFaker<Product>, ProductFaker>();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.Use(async (httpContext, next) =>
{
    Console.WriteLine("Before Use1");
    await next(httpContext);
    Console.WriteLine("After Use1");
});
app.Use(async (httpContext, next) =>
{
    Console.WriteLine("Before Use2");
    await next(httpContext);
    Console.WriteLine("After Use2");
});


var values = new List<int>
{
    1, 5, 8, 23, 456
};
//Minimal API
app.MapGet("/values", () => values);
app.MapDelete("/values/{value:int}", /*[Authorize]*/ (int value) => values.Remove(value));
app.MapPost("/values/{value:int:max(50)}", (int value) => values.Add(value));
app.MapPut("/values/{oldValue:int}/{newValue:int}", (int oldValue, int newValue) =>  values[values.IndexOf(oldValue)] = newValue);

app.MapControllers();

app.Run();





