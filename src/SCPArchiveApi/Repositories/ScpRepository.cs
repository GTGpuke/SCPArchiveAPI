using MongoDB.Driver;
using SCPArchiveApi.Models;

namespace SCPArchiveApi.Repositories;

/// <summary>
/// Implémentation MongoDB du repository SCP
/// </summary>
public class ScpRepository : IScpRepository
{
    private readonly IMongoCollection<ScpEntry> _collection;

    public ScpRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<ScpEntry>("scps");
        CreateIndexes();
    }

    private async void CreateIndexes()
    {
        var indexKeys = Builders<ScpEntry>.IndexKeys
            .Text(x => x.Title)
            .Text(x => x.Content);

        await _collection.Indexes.CreateOneAsync(
            new CreateIndexModel<ScpEntry>(indexKeys));
    }

    public async Task<ScpEntry?> GetByNumberAsync(string itemNumber)
    {
        var result = await _collection.Find(x => x.ItemNumber == itemNumber)
            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<(IEnumerable<ScpEntry> Items, int Total)> GetAllAsync(int skip, int take)
    {
        var filter = Builders<ScpEntry>.Filter.Empty;
        var total = await _collection.CountDocumentsAsync(filter);
        var items = await _collection.Find(filter)
            .Skip(skip)
            .Limit(take)
            .ToListAsync();

        return (items, (int)total);
    }

    public Task UpsertAsync(ScpEntry entry)
    {
        var filter = Builders<ScpEntry>.Filter
            .Eq(x => x.ItemNumber, entry.ItemNumber);

        return _collection.ReplaceOneAsync(
            filter,
            entry,
            new ReplaceOptions { IsUpsert = true });
    }

    public Task<IEnumerable<ScpEntry>> SearchAsync(
        string query,
        string? objectClass = null,
        int skip = 0,
        int take = 20)
    {
        var builder = Builders<ScpEntry>.Filter;
        var filter = builder.Text(query);

        if (!string.IsNullOrEmpty(objectClass))
        {
            filter &= builder.Eq(x => x.ObjectClass, objectClass);
        }

        return _collection.Find(filter)
            .SortByDescending(x => x.Metadata.Rating)
            .Skip(skip)
            .Limit(take)
            .ToListAsync()
            .ContinueWith(t => (IEnumerable<ScpEntry>)t.Result);
    }

    public async Task TestConnectionAsync()
    {
        // On exécute une commande ping pour vérifier la connexion
        await _collection.Database.RunCommandAsync<MongoDB.Bson.BsonDocument>(
            new MongoDB.Bson.BsonDocument("ping", 1));
    }
}
