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
    /// Ürün adı veya mağaza adına göre tam veya kısmi eşleşme ile arama yapar.
    /// PostgreSQL ILIKE kullanılır; Türkçe karakterlerde locale bağımsız çalışır.
    /// </summary>
    public async Task<List<Product>> SearchAsync(string query)
    {
        var exact = await _db.Products
            .Where(p => EF.Functions.ILike(p.Name, query) || EF.Functions.ILike(p.StoreName, query))
            .ToListAsync();

        if (exact.Count > 0)
            return exact;

        var pattern = $"%{query}%";
        return await _db.Products
            .Where(p => EF.Functions.ILike(p.Name, pattern) || EF.Functions.ILike(p.StoreName, pattern))
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
