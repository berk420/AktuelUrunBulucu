using AktuelUrunBulucu.DAL.Repositories;

namespace AktuelUrunBulucu.BLL.Services;

public class RecommendationService : IRecommendationService
{
    private readonly IProductRepository _productRepo;

    public RecommendationService(IProductRepository productRepo)
    {
        _productRepo = productRepo;
    }

    /// <summary>
    /// Verilen ilgi alanı listesine göre ürün önerilerini döner.
    /// Her ilgi alanı için veritabanındaki ürünlerin kategorisiyle eşleşme yapılır.
    /// </summary>
    public async Task<RecommendationResultDto> GetRecommendationsAsync(IEnumerable<string> interests)
    {
        var results = new List<InterestRecommendationDto>();

        foreach (var interest in interests.Where(i => !string.IsNullOrWhiteSpace(i)))
        {
            var products = await _productRepo.GetByCategoryKeywordAsync(interest);
            var dtos = products.Select(p => new RecommendationItemDto(
                p.Id,
                p.Name,
                p.Category,
                p.ProductBringDate,
                p.StoreName
            )).ToList();

            results.Add(new InterestRecommendationDto(interest, dtos));
        }

        return new RecommendationResultDto(results);
    }
}
