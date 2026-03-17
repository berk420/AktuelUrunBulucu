using AktuelUrunBulucu.DAL.Entities;

namespace AktuelUrunBulucu.DAL.Repositories;

public interface IUserCoordinateRepository
{
    Task SaveAsync(UserCoordinate coordinate);
}
