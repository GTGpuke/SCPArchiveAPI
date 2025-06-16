using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using SCPArchiveApi.Models;
using SCPArchiveApi.Repositories;

namespace SCPArchiveApi.Services;

/// <summary>
/// Service de scraping des articles SCP
/// </summary>
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

        // Configuration du User-Agent
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

    public async Task ScrapeAllAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Démarrage du scraping complet");
        // TODO: Implémenter le scraping complet
        throw new NotImplementedException();
    }

    public async Task CheckUpdatesAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Vérification des mises à jour");
        // TODO: Implémenter la vérification des mises à jour
        throw new NotImplementedException();
    }

    private async Task<HtmlDocument> LoadDocumentAsync(string url)
    {
        // Respect du délai entre les requêtes
        await Task.Delay(_options.DelayBetweenRequests);

        var response = await _httpClient.GetStringAsync(url);
        var doc = new HtmlDocument();
        doc.LoadHtml(response);
        return doc;
    }

    private async Task<ScpEntry> ParseScpEntry(HtmlDocument doc, string itemNumber)
    {
        // TODO: Implémenter le parsing HTML
        throw new NotImplementedException();
    }
}
