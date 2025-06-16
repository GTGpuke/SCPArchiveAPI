// Scraper : Parseur HTML pour extraire les informations d'un article SCP à partir du HTML du wiki
// Utilise HtmlAgilityPack pour naviguer dans le DOM et extraire les champs utiles

using HtmlAgilityPack;
using SCPArchiveApi.Models;

namespace SCPArchiveApi.Scraper;

/// <summary>
/// Parseur HTML pour les articles SCP (extraction du contenu, des métadonnées, etc.)
/// </summary>
public class ScpHtmlParser
{
    /// <summary>
    /// Parse le HTML d'un article SCP et retourne un objet ScpEntry complet
    /// </summary>
    public ScpEntry ParseArticleAsync(string html, string itemNumber)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Extraction du contenu principal de la page
        var mainContent = doc.DocumentNode.SelectSingleNode("//div[@id='page-content']");
        if (mainContent == null)
        {
            throw new Exception("Impossible de trouver le contenu principal");
        }

        // Extraction des métadonnées et du contenu
        var rating = ExtractRating(mainContent);
        var title = ExtractTitle(mainContent);
        var objectClass = ExtractObjectClass(mainContent);

        // Création de l'entrée SCP à partir des données extraites
        var entry = new ScpEntry
        {
            ItemNumber = itemNumber,
            Title = title,
            ObjectClass = objectClass,
            Content = new ScpContent
            {
                Description = ExtractDescription(mainContent),
                Containment = ExtractContainment(mainContent),
                Addenda = ExtractAddenda(mainContent)
            },
            Metadata = new ScpMetadata
            {
                Rating = rating,
                Author = ExtractAuthor(doc),
                CreationDate = ExtractCreationDate(doc),
                LastModified = DateTime.UtcNow,
                Tags = ExtractTags(doc)
            },
            Scraping = new ScrapingInfo
            {
                LastScraped = DateTime.UtcNow,
                SourceUrl = $"http://www.scp-wiki.net/{itemNumber}",
                Version = "1.0"
            }
        };

        return entry;
    }

    /// <summary>
    /// Extrait le titre de l'article
    /// </summary>
    private string ExtractTitle(HtmlNode mainContent)
    {
        var titleNode = mainContent.SelectSingleNode(".//h1");
        return titleNode?.InnerText.Trim() ?? "Titre inconnu";
    }

    /// <summary>
    /// Extrait la classe d'objet (Object Class)
    /// </summary>
    private string ExtractObjectClass(HtmlNode mainContent)
    {
        var text = mainContent.InnerText;
        var match = System.Text.RegularExpressions.Regex.Match(text, @"Object Class:\s*(\w+)");
        return match.Success ? match.Groups[1].Value : "Unknown";
    }

    /// <summary>
    /// Extrait la description principale
    /// </summary>
    private string ExtractDescription(HtmlNode mainContent)
    {
        var descriptionNode = mainContent.SelectSingleNode(".//p");
        return descriptionNode?.InnerText.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Extrait les procédures de confinement
    /// </summary>
    private string ExtractContainment(HtmlNode mainContent)
    {
        var containmentNode = mainContent.SelectSingleNode(
            "//*[contains(text(), 'Containment Procedures:')]//following-sibling::p[1]");
        return containmentNode?.InnerText.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Extrait les addenda (documents additionnels)
    /// </summary>
    private List<ScpAddendum> ExtractAddenda(HtmlNode mainContent)
    {
        var addenda = new List<ScpAddendum>();
        var addendumNodes = mainContent.SelectNodes("//div[contains(@class, 'addendum')]");

        if (addendumNodes != null)
        {
            foreach (var node in addendumNodes)
            {
                addenda.Add(new ScpAddendum
                {
                    Title = node.SelectSingleNode(".//strong")?.InnerText.Trim() ?? "Addendum",
                    Content = node.InnerText.Trim()
                });
            }
        }

        return addenda;
    }

    /// <summary>
    /// Extrait la note (rating) de l'article
    /// </summary>
    private int ExtractRating(HtmlNode mainContent)
    {
        var ratingNode = mainContent.SelectSingleNode("//span[@class='rate-points']");
        if (ratingNode != null && int.TryParse(ratingNode.InnerText, out var rating))
        {
            return rating;
        }
        return 0;
    }

    /// <summary>
    /// Extrait l'auteur de l'article
    /// </summary>
    private string ExtractAuthor(HtmlDocument doc)
    {
        var authorNode = doc.DocumentNode.SelectSingleNode("//div[@class='author']//a");
        return authorNode?.InnerText.Trim() ?? "Auteur inconnu";
    }

    /// <summary>
    /// Extrait la date de création de l'article
    /// </summary>
    private DateTime ExtractCreationDate(HtmlDocument doc)
    {
        var dateNode = doc.DocumentNode.SelectSingleNode("//div[@class='creation-date']");
        if (dateNode != null && DateTime.TryParse(dateNode.InnerText, out var date))
        {
            return date;
        }
        return DateTime.UtcNow;
    }

    /// <summary>
    /// Extrait les tags associés à l'article
    /// </summary>
    private List<string> ExtractTags(HtmlDocument doc)
    {
        var tags = new List<string>();
        var tagNodes = doc.DocumentNode.SelectNodes("//div[@class='page-tags']//a");

        if (tagNodes != null)
        {
            foreach (var node in tagNodes)
            {
                tags.Add(node.InnerText.Trim());
            }
        }

        return tags;
    }
}
