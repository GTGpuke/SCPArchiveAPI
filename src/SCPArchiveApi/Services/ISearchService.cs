using SCPArchiveApi.Models;

namespace SCPArchiveApi.Services;

/// <summary>
/// Interface pour le service de recherche
/// </summary>
public interface ISearchService
{
    /// <summary>
    /// Recherche des articles SCP par texte
    /// </summary>
    Task<SearchResult> SearchAsync(SearchQuery query);

    /// <summary>
    /// Suggestions de recherche basées sur l'historique
    /// </summary>
    Task<IEnumerable<string>> GetSuggestionsAsync(string partialQuery);
}
