using AktuelUrunBulucu.BLL.Services;
using AktuelUrunBulucu.DAL.Entities;
using AktuelUrunBulucu.DAL.Repositories;

namespace AktuelUrunBulucu.Endpoints;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/notify", async (NotifyRequest req, INotificationRequestRepository repo, IMailService mailService) =>
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

            await mailService.SendNotificationConfirmationAsync(req.Email, req.Product);

            return Results.Ok();
        })
        .WithName("SubscribeNotification")
        .WithTags("Notification");
    }
}

record NotifyRequest(string Ip, string Email, string Product);
