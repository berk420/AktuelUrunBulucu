using AktuelUrunBulucu.DAL.Entities;

namespace AktuelUrunBulucu.DAL.Repositories;

public interface INotificationRequestRepository
{
    Task AddAsync(NotificationRequest request);
}
