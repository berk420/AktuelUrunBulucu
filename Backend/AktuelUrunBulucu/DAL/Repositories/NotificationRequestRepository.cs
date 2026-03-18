using AktuelUrunBulucu.DAL.Context;
using AktuelUrunBulucu.DAL.Entities;

namespace AktuelUrunBulucu.DAL.Repositories;

public class NotificationRequestRepository : INotificationRequestRepository
{
    private readonly AppDbContext _db;

    public NotificationRequestRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(NotificationRequest request)
    {
        _db.NotificationRequests.Add(request);
        await _db.SaveChangesAsync();
    }
}
