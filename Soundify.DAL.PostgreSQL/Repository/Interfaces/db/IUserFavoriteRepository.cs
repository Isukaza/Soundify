using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;

namespace Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

public interface IUserFavoriteRepository : IDbRepositoryBase<UserFavorite>
{
    Task<UserFavorite> GetFavoriteByIdAsync(Guid favoriteId);
    IQueryable<UserFavorite> GetUserFavoriteByUserIdAsync(Guid userFavoriteId);
    Task<bool> FavoriteExistsAsync(Guid userId, Guid trackId);
}