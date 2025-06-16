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
    }

    public async Task<ScpEntry?> GetByNumberAsync(string itemNumber)
    {
        return await _collection.Find(x => x.ItemNumber == itemNumber).FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<ScpEntry> Items, int Total)> GetAllAsync(int page, int pageSize)
    {
        var filter = Builders<ScpEntry>.Filter.Empty;
        var total = await _collection.CountDocumentsAsync(filter);
        var items = await _collection.Find(filter)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (items, (int)total);
    }

    public async Task UpsertAsync(ScpEntry entry)
    {
        var filter = Builders<ScpEntry>.Filter.Eq(x => x.ItemNumber, entry.ItemNumber);
        await _collection.ReplaceOneAsync(filter, entry, new ReplaceOptions { IsUpsert = true });
    }

    public async Task<IEnumerable<ScpEntry>> SearchAsync(string query, string? objectClass = null)
    {
        var builder = Builders<ScpEntry>.Filter;
        var filter = builder.Text(query);

        if (!string.IsNullOrEmpty(objectClass))
        {
            filter &= builder.Eq(x => x.ObjectClass, objectClass);
        }

        return await _collection.Find(filter)
            .SortByDescending(x => x.Metadata.Rating)
            .Limit(100)
            .ToListAsync();
    }
}
