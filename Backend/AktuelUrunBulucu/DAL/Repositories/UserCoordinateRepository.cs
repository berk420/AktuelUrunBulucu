using AktuelUrunBulucu.DAL.Context;
using AktuelUrunBulucu.DAL.Entities;

namespace AktuelUrunBulucu.DAL.Repositories;

public class UserCoordinateRepository : IUserCoordinateRepository
{
    private readonly AppDbContext _db;

    public UserCoordinateRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task SaveAsync(UserCoordinate coordinate)
    {
        _db.UserCoordinates.Add(coordinate);
        await _db.SaveChangesAsync();
    }
}
