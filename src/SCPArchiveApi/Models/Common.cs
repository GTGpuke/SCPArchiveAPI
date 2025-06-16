namespace SCPArchiveApi.Models;

/// <summary>
/// Informations de scraping d'un article
/// </summary>
public class ScrapingInfo
{
    public DateTime LastScraped { get; set; }
    public string SourceUrl { get; set; } = null!;
    public string Version { get; set; } = null!;
}

/// <summary>
/// Options de configuration du scraping
/// </summary>
public class ScrapingOptions
{
    public string BaseUrl { get; set; } = "http://www.scp-wiki.net";
    public int DelayBetweenRequests { get; set; } = 1000;
    public string UserAgent { get; set; } = "SCPArchive-Bot/1.0";
}

/// <summary>
/// Paramètres de recherche
/// </summary>
public class SearchQuery
{
    public string Text { get; set; } = null!;
    public string? ObjectClass { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

/// <summary>
/// Résultats de recherche
/// </summary>
public class SearchResult
{
    public IEnumerable<ScpEntry> Items { get; set; } = null!;
    public int Total { get; set; }
    public SearchQuery Query { get; set; } = null!;
}
