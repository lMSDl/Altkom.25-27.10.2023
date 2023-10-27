using AutoMapper;
using GrpcService.AutoMapper;
using GrpcService.Services;
using Services.Bogus.Fakers;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddSingleton<IPeopleService, Services.Bogus.PeopleService>();
builder.Services.AddTransient<EntityFaker<Models.Person>, PersonFaker>();

builder.Services.AddSingleton(new MapperConfiguration(x => x.AddProfile<PersonMappingProfile>()).CreateMapper());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<PeopleService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
