using Quartz;
using SCPArchiveApi.Services;

namespace SCPArchiveApi.Scraper.Jobs;

/// <summary>
/// Job Quartz pour le scraping périodique des articles
/// </summary>
[DisallowConcurrentExecution]
public class ScrapingJob : IJob
{
    private readonly IScrapingService _scrapingService;
    private readonly ILogger<ScrapingJob> _logger;

    public ScrapingJob(IScrapingService scrapingService, ILogger<ScrapingJob> logger)
    {
        _scrapingService = scrapingService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Démarrage du job de scraping");
            
            // Vérifie les mises à jour des articles existants
            await _scrapingService.CheckUpdatesAsync(context.CancellationToken);
            
            _logger.LogInformation("Job de scraping terminé avec succès");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de l'exécution du job de scraping");
            throw;
        }
    }
}
