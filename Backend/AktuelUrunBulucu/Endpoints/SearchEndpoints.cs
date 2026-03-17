using AktuelUrunBulucu.BLL.Services;

namespace AktuelUrunBulucu.Endpoints;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/search", async (string query, string ip, ISearchService searchService) =>
        {
            if (string.IsNullOrWhiteSpace(query))
                return Results.BadRequest("query parametresi boş olamaz.");

            var result = await searchService.SearchAsync(query, ip);
            return Results.Ok(result);
        })
        .WithName("SearchProducts")
        .WithTags("Search");
    }
}
