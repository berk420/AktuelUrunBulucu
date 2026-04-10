namespace AktuelUrunBulucu.BLL.Services;

/// <summary>
/// Bir ilgi alanına ait aktüel ürün sonucunu temsil eder.
/// </summary>
public record RecommendationItemDto(int ProductId, string ProductName, string Category, DateTime? ProductBringDate, string StoreName);

/// <summary>
/// Tek bir ilgi alanı için eşleşen ürün listesini temsil eder.
/// </summary>
public record InterestRecommendationDto(string Interest, List<RecommendationItemDto> Products);

/// <summary>
/// Tüm ilgi alanlarına ait öneri sonuçlarını kapsar.
/// </summary>
public record RecommendationResultDto(List<InterestRecommendationDto> Results);

public interface IRecommendationService
{
    /// <summary>
    /// Verilen ilgi alanı listesine göre ürün önerilerini döner.
    /// </summary>
    Task<RecommendationResultDto> GetRecommendationsAsync(IEnumerable<string> interests);
}
