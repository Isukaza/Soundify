using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Base;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

namespace Soundify.DAL.PostgreSQL.Repository.db;

public class UserFavoriteRepository : DbRepositoryBase<UserFavorite>, IUserFavoriteRepository
{
    #region C-tor

    public UserFavoriteRepository(SoundifyDbContext dbContext) : base(dbContext)
    { }

    #endregion

    public async Task<UserFavorite> GetFavoriteByIdAsync(Guid favoriteId) =>
        await DbContext.UserFavorites.FirstOrDefaultAsync(f => f.Id == favoriteId);

    public IQueryable<UserFavorite> GetUserFavoriteByUserIdAsync(Guid userFavoriteId) =>
        DbContext.UserFavorites.Where(uf => uf.UserId == userFavoriteId);

    public async Task<bool> FavoriteExistsAsync(Guid userId, Guid trackId) =>
        await DbContext.Tracks
            .AsNoTracking()
            .AnyAsync(t => t.Id == trackId && t.UserFavorites.Any(uf => uf.UserId == userId));
}