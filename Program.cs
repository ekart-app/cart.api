using cart.api.Repository;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Configure detailed logging for Docker
builder.Logging.ClearProviders();
builder.Logging.AddConsole(options =>
{
    options.IncludeScopes = true; // Include scopes for context
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
});
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information); // Deepest level

// Ensure HTTP logging middleware logs at Information level
builder.Logging.AddFilter("Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware", LogLevel.Information);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddStackExchangeRedisCache(Options =>
{
    Options.Configuration = "redis";
    Options.InstanceName = "cart.api.redis";
});
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.MediaTypeOptions.AddText("application/json");
    logging.MediaTypeOptions.AddText("text/plain");
});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
