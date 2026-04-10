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

    /// <summary>
    /// Ürün adına göre tam veya kısmi eşleşme ile arama yapar.
    /// </summary>
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

    /// <summary>
    /// Kategori alanında verilen anahtar kelimeyi içeren ürünleri döner.
    /// </summary>
    public async Task<List<Product>> GetByCategoryKeywordAsync(string keyword)
    {
        var lower = keyword.ToLower();
        return await _db.Products
            .Where(p => p.Category.ToLower().Contains(lower))
            .ToListAsync();
    }

    /// <summary>
    /// Veritabanındaki tüm ürünleri döner.
    /// </summary>
    public async Task<List<Product>> GetAllAsync()
    {
        return await _db.Products.OrderBy(p => p.StoreName).ThenBy(p => p.Name).ToListAsync();
    }
}
