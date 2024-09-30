using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestPath
                            | HttpLoggingFields.RequestBody
                            | HttpLoggingFields.ResponseBody
                            | HttpLoggingFields.Duration
                            | HttpLoggingFields.ResponseStatusCode;
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "current",
        new OpenApiInfo
        {
            Title = "Music Service API",
            Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString()
        });

    options.IncludeXmlComments(
        Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

var app = builder.Build();

app.UseHttpLogging();

app.UseSwagger(options =>
{
    options.RouteTemplate = "/api/{documentName}/swagger.json";
    options.PreSerializeFilters.Add((document, _) => document.Servers.Clear());
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/api/current/swagger.json", "Music Service API");
    options.RoutePrefix = "api";
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();