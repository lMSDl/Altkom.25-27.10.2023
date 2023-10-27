using HealthChecks.Services;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddSqlServer("Data Source=(local)\\SQLExpress;Database=dotnet;Integrated security=true;TrustServerCertificate=True")
    .AddCheck<DirectoryAccessHealth>(nameof(DirectoryAccessHealth));
builder.Services.AddHealthChecksUI().AddInMemoryStorage();


var app = builder.Build();



app.MapHealthChecks("/Health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI();

app.MapGet("/", () => "Hello World!");


app.Run();
