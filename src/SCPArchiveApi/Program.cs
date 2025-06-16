using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using SCPArchiveApi.Models;
using SCPArchiveApi.Services;
using SCPArchiveApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SCP Archive API", Version = "v1" });
});

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ScpEntryDtoValidator>();

// Configuration des options
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<ScrapingOptions>(
    builder.Configuration.GetSection("Scraping"));

// Configuration MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    return client.GetDatabase(settings.DatabaseName);
});

// Services HTTP
builder.Services.AddHttpClient<IScrapingService, ScrapingService>();

// Services métier
builder.Services.AddScoped<IScpRepository, ScpRepository>();
builder.Services.AddScoped<IScrapingService, ScrapingService>();
builder.Services.AddScoped<ISearchService, SearchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add custom middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
