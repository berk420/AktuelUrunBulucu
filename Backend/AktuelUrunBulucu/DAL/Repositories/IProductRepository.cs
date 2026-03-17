using AktuelUrunBulucu.DAL.Entities;

namespace AktuelUrunBulucu.DAL.Repositories;

public interface IProductRepository
{
    Task<List<Product>> SearchAsync(string query);
}
