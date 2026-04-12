using AktuelUrunBulucu.DAL.Entities;
using AktuelUrunBulucu.DAL.Repositories;

namespace AktuelUrunBulucu.BLL.Services;

public class SearchService : ISearchService
{
    private readonly IProductRepository _productRepo;
    private readonly ISearchLogRepository _logRepo;
    private readonly ILogger<SearchService> _logger;

    public SearchService(IProductRepository productRepo, ISearchLogRepository logRepo, ILogger<SearchService> logger)
    {
        _productRepo = productRepo;
        _logRepo = logRepo;
        _logger = logger;
    }

    /// <summary>
    /// Verilen sorgu ile ürün araması yapar ve arama logunu kaydeder.
    /// </summary>
    public async Task<SearchResultDto> SearchAsync(string query, string ipAddress)
    {
        _logger.LogInformation("Ürün araması başladı. Query={Query} IP={IP}", query, ipAddress);

        List<DAL.Entities.Product> products;
        try
        {
            products = await _productRepo.SearchAsync(query);
            _logger.LogInformation("DB sorgusu tamamlandı. Query={Query} EşleşenAdet={Count}", query, products.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün araması DB hatası. Query={Query} ExceptionType={Type}", query, ex.GetType().Name);
            throw;
        }

        try
        {
            await _logRepo.AddAsync(new SearchLog
            {
                IpAddress = ipAddress,
                SearchedProduct = query,
                SearchedAt = DateTime.UtcNow
            });
            _logger.LogDebug("Arama logu kaydedildi. Query={Query} IP={IP}", query, ipAddress);
        }
        catch (Exception ex)
        {
            // Log kaydı başarısız olsa da aramayı engelleme; sadece logla
            _logger.LogWarning(ex, "Arama logu kaydedilemedi. Query={Query} IP={IP}", query, ipAddress);
        }

        if (products.Count == 0)
        {
            _logger.LogInformation("Sonuç bulunamadı. Query={Query}", query);
            return new SearchResultDto(false, []);
        }

        var matched = products.Select(p => new ProductResultDto(
            p.Id,
            p.Name,
            p.Category,
            p.ProductBringDate,
            p.StoreName
        )).ToList();

        _logger.LogInformation("Arama başarılı. Query={Query} SonuçAdet={Count}", query, matched.Count);
        return new SearchResultDto(true, matched);
    }
}
