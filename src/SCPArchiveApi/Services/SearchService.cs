using SCPArchiveApi.Models;
using SCPArchiveApi.Repositories;

namespace SCPArchiveApi.Services;

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
            var results = await _repository.SearchAsync(
                query.Text,
                query.ObjectClass,
                skip: (query.Page - 1) * query.PageSize,
                take: query.PageSize
            );

            return new SearchResult
            {
                Items = results,
                Total = results.Count(),
                Query = query
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur de recherche: {Query}", query.Text);
            throw;
        }
    }

    public Task<IEnumerable<string>> GetSuggestionsAsync(string partialQuery)
    {
        if (string.IsNullOrWhiteSpace(partialQuery))
            return Task.FromResult<IEnumerable<string>>(Array.Empty<string>());

        return Task.FromResult<IEnumerable<string>>(new[] {
            $"SCP-{partialQuery}",
            $"Class {partialQuery}"
        });
    }
}
