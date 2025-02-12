using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StealAllTheCats.API.Endpoints;
using StealAllTheCats.API.Interfaces;
using StealAllTheCats.API.Persistence;
using StealAllTheCats.API.Repositories;
using StealAllTheCats.API.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName ?? "Production";

// Set configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Load configuration
var configuration = builder.Configuration;

// Add SQL Server DB Context (runtime configuration)
builder.Services.AddDbContext<StealAllTheCatsContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Repository service for cats
builder.Services.AddScoped<ICatRepository, CatRepository>();

// Ensure the factory is only used for migrations
builder.Services.AddSingleton<IDesignTimeDbContextFactory<StealAllTheCatsContext>, StealAllTheCatsContextFactory>();

// Service for contacting cat API
builder.Services.AddHttpClient<ICatApiService, CatApiService>();

// Service for Cats
builder.Services.AddTransient<ICatService, CatService>();

// Background service that runs periodically to retrieve images
builder.Services.AddHostedService<ImageCacheService>();

// Database initializer 
builder.Services.AddHostedService<DatabaseInitializerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapCatEndpoints();

app.UseHttpsRedirection();

app.Run();
