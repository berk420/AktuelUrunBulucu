using AktuelUrunBulucu.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace AktuelUrunBulucu.Endpoints;

public static class RecommendationEndpoints
{
    public static void MapRecommendationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/recommendations", async ([FromQuery] string[] interests, IRecommendationService recommendationService) =>
        {
            if (interests == null || interests.Length == 0)
                return Results.BadRequest("interests parametresi boş olamaz.");

            var result = await recommendationService.GetRecommendationsAsync(interests);
            return Results.Ok(result);
        })
        .WithName("GetRecommendations")
        .WithTags("Recommendations");
    }
}
