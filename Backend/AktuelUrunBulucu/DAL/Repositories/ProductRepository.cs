using AktuelUrunBulucu.DAL.Context;
using AktuelUrunBulucu.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AktuelUrunBulucu.DAL.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> SearchAsync(string query)
    {
        var lower = query.ToLower();

        var exact = await _db.Products
            .Where(p => p.Name.ToLower() == lower)
            .ToListAsync();

        if (exact.Count > 0)
            return exact;

        return await _db.Products
            .Where(p => p.Name.ToLower().Contains(lower))
            .ToListAsync();
    }
}
