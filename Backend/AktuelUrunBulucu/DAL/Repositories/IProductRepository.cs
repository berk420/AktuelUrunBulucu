using AktuelUrunBulucu.DAL.Entities;

namespace AktuelUrunBulucu.DAL.Repositories;

public interface IProductRepository
{
    /// <summary>
    /// Ürün adına göre arama yapar.
    /// </summary>
    Task<List<Product>> SearchAsync(string query);

    /// <summary>
    /// Kategori alanında verilen anahtar kelimeyi içeren ürünleri döner.
    /// </summary>
    Task<List<Product>> GetByCategoryKeywordAsync(string keyword);
}
