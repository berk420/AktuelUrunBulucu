using AktuelUrunBulucu.DAL.Entities;
using AktuelUrunBulucu.DAL.Repositories;

namespace AktuelUrunBulucu.Endpoints;

public static class LocationEndpoints
{
    public static void MapLocationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/location", async (LocationRequest req, IUserCoordinateRepository repo) =>
        {
            await repo.SaveAsync(new UserCoordinate
            {
                IpAddress = req.Ip,
                Latitude = req.Latitude,
                Longitude = req.Longitude,
                SavedAt = DateTime.UtcNow
            });

            return Results.Ok();
        })
        .WithName("SaveLocation")
        .WithTags("Location");
    }
}

record LocationRequest(string Ip, double Latitude, double Longitude);
