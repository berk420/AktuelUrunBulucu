using AktuelUrunBulucu.DAL.Entities;
using AktuelUrunBulucu.DAL.Repositories;

namespace AktuelUrunBulucu.Endpoints;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/notify", async (NotifyRequest req, INotificationRequestRepository repo) =>
        {
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Product))
                return Results.BadRequest("Email ve ürün adı zorunludur.");

            await repo.AddAsync(new NotificationRequest
            {
                IpAddress = req.Ip,
                Email = req.Email,
                SearchedProduct = req.Product,
                RequestedAt = DateTime.UtcNow
            });

            return Results.Ok();
        })
        .WithName("SubscribeNotification")
        .WithTags("Notification");
    }
}

record NotifyRequest(string Ip, string Email, string Product);
