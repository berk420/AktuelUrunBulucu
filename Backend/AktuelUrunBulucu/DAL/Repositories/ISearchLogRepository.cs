using AktuelUrunBulucu.DAL.Entities;

namespace AktuelUrunBulucu.DAL.Repositories;

public interface ISearchLogRepository
{
    Task AddAsync(SearchLog log);
}
