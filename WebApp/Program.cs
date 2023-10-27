using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.Interfaces;
using System.Net.WebSockets;
using System.Text.Json.Serialization;
using WebApp.Filters;
using WebApp.Services;
using WebApp.SignalR;
using WebApp.Validators;

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


builder.Services.AddFluentValidationAutoValidation()
    //automatyczna rejestracja walidatorów z assembly zawieraj¹cego wskazan¹ klasê
    .AddValidatorsFromAssemblyContaining<Program>();

//rêczna rejestracja walidatorów
//builder.Services.AddTransient<IValidator<ShoppingList>, ShoppingListValidator>();

builder.Services.AddTransient<ConsoleLogFilter>();
builder.Services.AddSingleton<LimiterFilter>(x => new LimiterFilter(5));
builder.Services.AddTransient<UniquePersonFilter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WebAPI", Version = "v1" }))
    .AddSwaggerGenNewtonsoftSupport();

builder.Services.AddResponseCompression(x =>
{
    x.Providers.Clear();

    x.Providers.Add<GzipCompressionProvider>();
    x.Providers.Add<BrotliCompressionProvider>();

    x.EnableForHttps = true;
});

builder.Services.Configure<GzipCompressionProviderOptions>(x => x.Level = System.IO.Compression.CompressionLevel.Optimal);
builder.Services.Configure<BrotliCompressionProviderOptions>(x => x.Level = System.IO.Compression.CompressionLevel.Optimal);


builder.Services.AddTransient<AuthService>();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(AuthService.Key),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "SwaggerWebApi v1"));

app.UseResponseCompression();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (httpContext, next) =>
{
    Console.WriteLine("Before Use1");
    await next(httpContext);
    Console.WriteLine("After Use1");
});
/*app.Use(async (httpContext, next) =>
{
    if (httpContext.GetEndpoint()?.DisplayName.Contains("ShoppingListsController.Post") ?? false)
    Console.WriteLine("Before " + httpContext.Request.Method);
    await next(httpContext);
    if (httpContext.GetEndpoint()?.DisplayName.Contains("ShoppingListsController.Post") ?? false)
        Console.WriteLine("After " + httpContext.Request.Method);
});*/


app.MapHub<DemoHub>("SignalR/Demo");


var values = new List<int>
{
    1, 5, 8, 23, 456
};

//Minimal API
app.MapGet("/values", () => values);
app.MapDelete("/values/{value:int}", /*[Authorize]*/ (int value) => values.Remove(value));
app.MapPost("/values/{value:int:max(50)}", (int value) => values.Add(value));
app.MapPut("/values/{oldValue:int}/{newValue:int}", (int oldValue, int newValue) =>  values[values.IndexOf(oldValue)] = newValue);

app.MapGet("/login", (string login, string password, AuthService authService) => authService.Authenticate(login, password));


app.MapControllers();

app.Run();





