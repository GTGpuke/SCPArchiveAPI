using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using SCPArchiveApi.Models;
using SCPArchiveApi.Services;
using SCPArchiveApi.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using SCPArchiveApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

try
{
    var mongoSettings = builder.Configuration.GetSection("MongoDB").Get<MongoSettings>();
    if (mongoSettings == null)
    {
        throw new Exception("La configuration MongoDB est manquante dans appsettings.json");
    }

    builder.Services.AddControllers();
    builder.Services.AddFluentValidationAutoValidation()
        .AddFluentValidationClientsideAdapters();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "SCP Archive API", Version = "v1" }));

    builder.Services.AddApiVersioning(opt => {
        opt.DefaultApiVersion = new ApiVersion(1, 0);
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.ReportApiVersions = true;
    });

    builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoDB"));
    builder.Services.Configure<ScrapingOptions>(builder.Configuration.GetSection("Scraping"));

    // Test de la connexion MongoDB
    var mongoClient = new MongoClient(mongoSettings.ConnectionString);
    var mongoDatabase = mongoClient.GetDatabase(mongoSettings.DatabaseName);
    // Vérifie que la base est accessible
    mongoDatabase.RunCommand<MongoDB.Bson.BsonDocument>(new MongoDB.Bson.BsonDocument("ping", 1));

    builder.Services.AddSingleton<IMongoClient>(sp => mongoClient);
    builder.Services.AddScoped<IMongoDatabase>(sp => mongoDatabase);

    builder.Services.AddHttpClient<IScrapingService, ScrapingService>();
    builder.Services.AddScoped<IScpRepository, ScpRepository>();
    builder.Services.AddScoped<IScrapingService, ScrapingService>();
    builder.Services.AddScoped<ISearchService, SearchService>();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseMiddleware<ExceptionHandlingMiddleware>()
       .UseMiddleware<RateLimitingMiddleware>()
       .UseHttpsRedirection()
       .UseAuthorization();

    app.MapControllers();
    app.MapGet("/", context => {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });

    app.Run();
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Erreur fatale au démarrage : {ex.Message}");
    Console.Error.WriteLine(ex.StackTrace);
    throw;
}
