using SCPArchiveApi.Models;
using SCPArchiveApi.Repositories;

namespace SCPArchiveApi.Services;

/// <summary>
/// Service de recherche dans les articles SCP
/// </summary>
public class SearchService : ISearchService
{
    private readonly IScpRepository _repository;
    private readonly ILogger<SearchService> _logger;

    public SearchService(IScpRepository repository, ILogger<SearchService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<SearchResult> SearchAsync(SearchQuery query)
    {
        try
        {
            var results = await _repository.SearchAsync(query.Text, query.ObjectClass);
            return new SearchResult
            {
                Items = results,
                Total = results.Count(),
                Query = query
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la recherche avec la requête {Query}", query.Text);
            throw;
        }
    }

    public async Task<IEnumerable<string>> GetSuggestionsAsync(string partialQuery)
    {
        // TODO: Implémenter les suggestions
        return Array.Empty<string>();
    }
}
