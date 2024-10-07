using System.Reflection;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Soundify.Configuration;
using Soundify.DAL.PostgreSQL;
using Soundify.DAL.PostgreSQL.Repository.Base;
using Soundify.DAL.PostgreSQL.Repository.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;
using Soundify.DAL.PostgreSQL.Roles;
using Soundify.Managers;
using Soundify.Managers.Interfaces;

using StackExchange.Redis;

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

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(nameof(RolePolicy.RequireAnyAdminOrPublisher), policy =>
        policy.RequireRole(nameof(UserRole.SuperAdmin), nameof(UserRole.Admin), nameof(UserRole.Publisher)))
    .AddPolicy(nameof(RolePolicy.RequireAnyAdmin), policy =>
        policy.RequireRole(nameof(UserRole.SuperAdmin), nameof(UserRole.Admin)))
    .AddPolicy(nameof(RolePolicy.RequireContentEditors), policy =>
        policy.RequireRole(nameof(UserRole.SuperAdmin), nameof(UserRole.Admin), nameof(UserRole.Manager),
            nameof(UserRole.Publisher)));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = JwtConfig.Values.Issuer,
            ValidateAudience = true,
            ValidAudience = JwtConfig.Values.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = JwtConfig.Values.Key,
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddDbContext<SoundifyDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");
    options.UseNpgsql(connectionString);
});

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    var redisConnectionUrl = builder.Configuration.GetConnectionString("Redis");
    if (string.IsNullOrEmpty(redisConnectionUrl))
        throw new InvalidOperationException("RedisConnectionUrl is not configured.");

    return ConnectionMultiplexer.Connect(redisConnectionUrl);
});

builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IArtistSmRepository, ArtistSmRepository>();
builder.Services.AddScoped<IPlayListRepository, PlayListRepository>();
builder.Services.AddScoped<IPlayListTrackRepository, PlayListTrackRepository>();
builder.Services.AddScoped<ISingleRepository, SingleRepository>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<ITrackRatingRepository, TrackRatingRepository>();
builder.Services.AddScoped<IUserFavoriteRepository, UserFavoriteRepository>();
builder.Services.AddScoped<ICacheRepositoryBase, CacheRepositoryBase>();

builder.Services.AddScoped<IAlbumManager, AlbumManager>();
builder.Services.AddScoped<IGenreManager, GenreManager>();
builder.Services.AddScoped<IArtistManager, ArtistManager>();
builder.Services.AddScoped<IArtistSmManager, ArtistSmManager>();
builder.Services.AddScoped<IPlayListManager, PlayListManager>();
builder.Services.AddScoped<ISingleManager, SingleManager>();
builder.Services.AddScoped<ITrackManager, TrackManager>();
builder.Services.AddScoped<ITrackRatingManager, TrackRatingManager>();
builder.Services.AddScoped<IUserFavoriteManager, UserFavoriteManager>();

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
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
            "Enter 'Bearer' [space and then your token in the text input below. \r\n\r\n" +
            "Example: 'Bearer HHH.PPP.CCC'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "OAuth 2.0",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();