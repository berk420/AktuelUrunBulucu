namespace AktuelUrunBulucu.BLL.Services;

public record ProductResultDto(int ProductId, string ProductName, string Category, DateTime? ProductBringDate, string StoreName);
public record SearchResultDto(bool Found, List<ProductResultDto> MatchedProducts);

public interface ISearchService
{
    Task<SearchResultDto> SearchAsync(string query, string ipAddress);
}
