// Contrôleur API versionné pour la gestion des articles SCP
// Gère les endpoints REST pour la récupération, la recherche et le scraping des articles

using Microsoft.AspNetCore.Mvc;
using SCPArchiveApi.DTOs;
using SCPArchiveApi.Services;

namespace SCPArchiveApi.Controllers.V1;

/// <summary>
/// Contrôleur pour la gestion des articles SCP (version 1 de l'API)
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/scps")]
public class ScpsController : ControllerBase
{
    // Dépendances injectées : repository, services de scraping et de recherche, logger
    private readonly IScpRepository _repository;
    private readonly IScrapingService _scrapingService;
    private readonly ISearchService _searchService;
    private readonly ILogger<ScpsController> _logger;

    public ScpsController(
        IScpRepository repository,
        IScrapingService scrapingService,
        ISearchService searchService,
        ILogger<ScpsController> logger)
    {
        _repository = repository;
        _scrapingService = scrapingService;
        _searchService = searchService;
        _logger = logger;
    }

    /// <summary>
    /// Récupère un article SCP par son numéro (ex: SCP-173)
    /// </summary>
    /// <param name="itemNumber">Numéro SCP</param>
    /// <returns>Article SCP détaillé ou 404 si non trouvé</returns>
    [HttpGet("{itemNumber}")]
    [ProducesResponseType(typeof(ScpEntryDetailedDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ScpEntryDetailedDto>> GetByNumber(string itemNumber)
    {
        var scp = await _repository.GetByNumberAsync(itemNumber);
        if (scp == null)
        {
            return NotFound();
        }

        // TODO: Ajouter le mapping vers DTO (ex: via AutoMapper)
        return Ok(scp);
    }

    /// <summary>
    /// Recherche des articles SCP avec pagination et filtres
    /// </summary>
    /// <param name="query">Texte de recherche</param>
    /// <param name="objectClass">Filtre par classe</param>
    /// <param name="page">Numéro de page</param>
    /// <param name="pageSize">Taille de page</param>
    /// <returns>Liste paginée d'articles SCP</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ScpEntryDetailedDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ScpEntryDetailedDto>>> Search(
        [FromQuery] string? query,
        [FromQuery] string? objectClass,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var searchQuery = new SearchQuery
        {
            Text = query ?? string.Empty,
            ObjectClass = objectClass,
            Page = page,
            PageSize = Math.Min(pageSize, 100)
        };

        var result = await _searchService.SearchAsync(searchQuery);
        // TODO: Ajouter le mapping vers DTO
        return Ok(result);
    }

    /// <summary>
    /// Déclenche le scraping d'un article SCP (asynchrone)
    /// </summary>
    /// <param name="itemNumber">Numéro SCP à scraper</param>
    /// <returns>202 Accepted si le scraping démarre</returns>
    [HttpPost("{itemNumber}/scrape")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Scrape(string itemNumber)
    {
        try
        {
            // Lance le scraping de manière asynchrone (fire & forget)
            _ = _scrapingService.ScrapeEntryAsync(itemNumber);
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du démarrage du scraping pour {ItemNumber}", itemNumber);
            return StatusCode(500, "Une erreur est survenue lors du démarrage du scraping");
        }
    }
}
