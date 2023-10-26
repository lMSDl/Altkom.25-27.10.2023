using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    /*.AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
        x.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.WriteAsString;
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })*/
    .AddNewtonsoftJson(x =>
    {
        x.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        x.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
        x.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
        x.SerializerSettings.DateFormatString = "yyy MM d ff:ss;mm";
    })
    .AddXmlSerializerFormatters(); //wsparcie dla XML

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


//zawieszenie automatycznej walidacji modelu
//builder.Services.Configure<ApiBehaviorOptions>(x => x.SuppressModelStateInvalidFilter = true); 

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





