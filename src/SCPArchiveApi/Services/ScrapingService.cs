using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using SCPArchiveApi.Models;
using SCPArchiveApi.Repositories;

namespace SCPArchiveApi.Services;

public class ScrapingService : IScrapingService
{
    private readonly IScpRepository _repository;
    private readonly HttpClient _httpClient;
    private readonly ILogger<ScrapingService> _logger;
    private readonly ScrapingOptions _options;

    public ScrapingService(
        IScpRepository repository,
        HttpClient httpClient,
        ILogger<ScrapingService> logger,
        IOptions<ScrapingOptions> options)
    {
        _repository = repository;
        _httpClient = httpClient;
        _logger = logger;
        _options = options.Value;
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_options.UserAgent);
    }

    public async Task ScrapeEntryAsync(string itemNumber)
    {
        try
        {
            var url = $"{_options.BaseUrl}/{itemNumber}";
            var doc = await LoadDocumentAsync(url);
            var entry = await ParseScpEntry(doc, itemNumber);
            await _repository.UpsertAsync(entry);
            _logger.LogInformation("Article {ItemNumber} scrapé avec succès", itemNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du scraping de {ItemNumber}", itemNumber);
            throw;
        }
    }

    public Task ScrapeAllAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Démarrage du scraping complet");
        throw new NotImplementedException("Le scraping complet n'est pas encore implémenté");
    }

    public Task CheckUpdatesAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Vérification des mises à jour");
        throw new NotImplementedException("La vérification des mises à jour n'est pas encore implémentée");
    }

    private async Task<HtmlDocument> LoadDocumentAsync(string url)
    {
        await Task.Delay(_options.DelayBetweenRequests);
        var response = await _httpClient.GetStringAsync(url);
        var doc = new HtmlDocument();
        doc.LoadHtml(response);
        return doc;
    }

    private Task<ScpEntry> ParseScpEntry(HtmlDocument doc, string itemNumber)
    {
        throw new NotImplementedException("Le parsing HTML n'est pas encore implémenté");
    }
}
