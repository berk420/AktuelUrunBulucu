using AktuelUrunBulucu.DAL.Context;
using AktuelUrunBulucu.DAL.Entities;

namespace AktuelUrunBulucu.DAL.Repositories;

public class SearchLogRepository : ISearchLogRepository
{
    private readonly AppDbContext _db;

    public SearchLogRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(SearchLog log)
    {
        _db.SearchLogs.Add(log);
        await _db.SaveChangesAsync();
    }
}
