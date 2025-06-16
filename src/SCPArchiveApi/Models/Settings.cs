namespace SCPArchiveApi.Models;

/// <summary>
/// Configuration de la base de données MongoDB
/// </summary>
public class MongoSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
