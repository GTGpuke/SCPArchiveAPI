namespace SCPArchiveApi.Models;

/// <summary>
/// Représente un article SCP dans la base de données
/// </summary>
public class ScpEntry
{
    public string Id { get; set; } = null!;
    public string ItemNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string ObjectClass { get; set; } = null!;
    public ScpContent Content { get; set; } = null!;
    public ScpMetadata Metadata { get; set; } = null!;
    public ScrapingInfo Scraping { get; set; } = null!;
}

/// <summary>
/// Contenu principal d'un article SCP
/// </summary>
public class ScpContent
{
    public string Description { get; set; } = null!;
    public string Containment { get; set; } = null!;
    public List<ScpAddendum> Addenda { get; set; } = new();
}

/// <summary>
/// Documents additionnels d'un article SCP
/// </summary>
public class ScpAddendum
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
}

/// <summary>
/// Métadonnées d'un article SCP
/// </summary>
public class ScpMetadata
{
    public string Author { get; set; } = null!;
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
    public int Rating { get; set; }
    public List<string> Tags { get; set; } = new();
}
