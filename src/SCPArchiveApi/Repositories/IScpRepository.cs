using SCPArchiveApi.Models;

namespace SCPArchiveApi.Repositories;

/// <summary>
/// Interface pour l'accès aux données des SCP dans MongoDB
/// </summary>
public interface IScpRepository
{
    /// <summary>
    /// Récupère un SCP par son numéro
    /// </summary>
    Task<ScpEntry?> GetByNumberAsync(string itemNumber);

    /// <summary>
    /// Récupère tous les SCPs avec pagination
    /// </summary>
    Task<(IEnumerable<ScpEntry> Items, int Total)> GetAllAsync(int skip, int take);

    /// <summary>
    /// Ajoute ou met à jour un SCP
    /// </summary>
    Task UpsertAsync(ScpEntry entry);

    /// <summary>
    /// Recherche des SCPs par critères
    /// </summary>
    Task<IEnumerable<ScpEntry>> SearchAsync(
        string query, 
        string? objectClass = null,
        int skip = 0,
        int take = 20);

    /// <summary>
    /// Teste la connexion à la base de données MongoDB
    /// </summary>
    Task TestConnectionAsync();
}
