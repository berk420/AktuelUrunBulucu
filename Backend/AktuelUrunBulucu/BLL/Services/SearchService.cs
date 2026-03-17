using AktuelUrunBulucu.DAL.Entities;
using AktuelUrunBulucu.DAL.Repositories;

namespace AktuelUrunBulucu.BLL.Services;

public class SearchService : ISearchService
{
    private readonly IProductRepository _productRepo;
    private readonly ISearchLogRepository _logRepo;

    public SearchService(IProductRepository productRepo, ISearchLogRepository logRepo)
    {
        _productRepo = productRepo;
        _logRepo = logRepo;
    }

    public async Task<SearchResultDto> SearchAsync(string query, string ipAddress)
    {
        var products = await _productRepo.SearchAsync(query);

        await _logRepo.AddAsync(new SearchLog
        {
            IpAddress = ipAddress,
            SearchedProduct = query,
            SearchedAt = DateTime.UtcNow
        });

        if (products.Count == 0)
            return new SearchResultDto(false, []);

        var matched = products.Select(p => new ProductResultDto(
            p.Id,
            p.Name,
            p.Category,
            p.ProductBringDate,
            p.StoreName
        )).ToList();

        return new SearchResultDto(true, matched);
    }
}
