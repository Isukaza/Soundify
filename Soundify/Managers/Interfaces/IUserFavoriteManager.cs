using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.Models.Request.Create;

namespace Soundify.Managers.Interfaces;

public interface IUserFavoriteManager
{
    Task<UserFavorite> GetFavoriteByIdAsync(Guid favoriteId);
    Task<List<UserFavorite>> GetFavoriteByUserIdAsync(Guid userFavoriteId);
    Task<UserFavorite> AddFavoriteAsync(UserFavoriteCreateRequest favoriteData);
    Task<bool> DeleteFavoriteAsync(UserFavorite favorite);

    public Task<bool> FavoriteExistsAsync(Guid userId, Guid trackId);
}