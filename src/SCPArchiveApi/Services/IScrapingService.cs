namespace SCPArchiveApi.Services;

/// <summary>
/// Interface pour le service de scraping des articles SCP
/// </summary>
public interface IScrapingService
{
    /// <summary>
    /// Lance le scraping d'un article SCP spécifique
    /// </summary>
    Task ScrapeEntryAsync(string itemNumber);

    /// <summary>
    /// Lance le scraping complet du site
    /// </summary>
    Task ScrapeAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Vérifie les mises à jour des articles existants
    /// </summary>
    Task CheckUpdatesAsync(CancellationToken cancellationToken);
}
