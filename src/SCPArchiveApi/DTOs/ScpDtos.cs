// DTOs (Data Transfer Objects) : Objets utilisés pour transférer les données entre l'API et le client, sans exposer les modèles internes de la base de données.

namespace SCPArchiveApi.DTOs;

/// <summary>
/// DTO pour la création ou la mise à jour d'un article SCP.
/// Utilisé lors des requêtes POST/PUT pour éviter d'exposer le modèle de base de données.
/// </summary>
public class ScpEntryDto
{
    /// <summary>
    /// Numéro unique de l'objet SCP (ex: SCP-173)
    /// </summary>
    public string ItemNumber { get; set; } = null!;
    /// <summary>
    /// Titre de l'article SCP
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// Classe de l'objet (Safe, Euclid, Keter, etc.)
    /// </summary>
    public string ObjectClass { get; set; } = null!;
    /// <summary>
    /// Contenu principal de l'article
    /// </summary>
    public ScpContentDto Content { get; set; } = null!;
    /// <summary>
    /// Liste des tags associés à l'article
    /// </summary>
    public List<string> Tags { get; set; } = new();
}

/// <summary>
/// Contenu principal d'un article SCP (description, confinement, addenda)
/// </summary>
public class ScpContentDto
{
    public string Description { get; set; } = null!;
    public string Containment { get; set; } = null!;
    public List<ScpAddendumDto> Addenda { get; set; } = new();
}

/// <summary>
/// Addendum (document additionnel) d'un article SCP
/// </summary>
public class ScpAddendumDto
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
}

/// <summary>
/// DTO pour la réponse détaillée d'un article SCP (inclut les métadonnées et infos de scraping)
/// </summary>
public class ScpEntryDetailedDto : ScpEntryDto
{
    /// <summary>
    /// Métadonnées de l'article (auteur, dates, note, etc.)
    /// </summary>
    public ScpMetadataDto Metadata { get; set; } = null!;
    /// <summary>
    /// Informations sur le scraping de l'article
    /// </summary>
    public ScpScrapingInfoDto Scraping { get; set; } = null!;
}

/// <summary>
/// Métadonnées d'un article SCP (auteur, dates, note, etc.)
/// </summary>
public class ScpMetadataDto
{
    public string Author { get; set; } = null!;
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
    public int Rating { get; set; }
}

/// <summary>
/// Informations sur le scraping d'un article SCP
/// </summary>
public class ScpScrapingInfoDto
{
    public DateTime LastScraped { get; set; }
    public string SourceUrl { get; set; } = null!;
}
