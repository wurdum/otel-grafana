using OtelApp.Instrumentation;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .AddSimpleConsole()
    .WithOtelLogging();

builder.Services.AddOpenTelemetry()
    .WithOtelResource()
    .WithOtelMetrics()
    .WithOtelTraces();

var app = builder.Build();

app.MapGet("/", (ILogger<Program> logger) =>
{
    ServiceMetrics.RequestHandled();
    ServiceTraces.RequestHandled();

    logger.LogInformation("Request is handled!");

    return "Hello World!";
});

app.Run();
