using AktuelUrunBulucu.BLL.Services;
using Npgsql;

namespace AktuelUrunBulucu.Endpoints;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/search", async (string query, string ip, ISearchService searchService, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SearchEndpoints");
            logger.LogInformation("Arama isteği alındı. Query={Query} IP={IP}", query, ip);

            if (string.IsNullOrWhiteSpace(query))
            {
                logger.LogWarning("Boş query ile istek geldi. IP={IP}", ip);
                return Results.BadRequest(new { error = "query parametresi boş olamaz." });
            }

            try
            {
                var result = await searchService.SearchAsync(query, ip);
                logger.LogInformation(
                    "Arama tamamlandı. Query={Query} Found={Found} ResultCount={Count}",
                    query, result.Found, result.MatchedProducts.Count);
                return Results.Ok(result);
            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex,
                    "Veritabanı bağlantı hatası. Query={Query} IP={IP} Message={Message}",
                    query, ip, ex.Message);

                return Results.Problem(
                    detail: "Veritabanına bağlanılamadı.",
                    title: "Bir sorun çıktı",
                    statusCode: 503);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Arama sırasında hata. Query={Query} IP={IP} ExceptionType={Type} Message={Message}",
                    query, ip, ex.GetType().Name, ex.Message);

                return Results.Problem(
                    detail: ex.Message,
                    title: "Arama sırasında sunucu hatası oluştu",
                    statusCode: 500,
                    extensions: new Dictionary<string, object?>
                    {
                        ["exceptionType"] = ex.GetType().Name,
                        ["innerMessage"] = ex.InnerException?.Message
                    });
            }
        })
        .WithName("SearchProducts")
        .WithTags("Search")
        .RequireRateLimiting("search");
    }
}
