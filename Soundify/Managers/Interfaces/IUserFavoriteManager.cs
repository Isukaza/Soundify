using Soundify.DAL.PostgreSQL.Models.db;

namespace Soundify.Managers.Interfaces;

public interface IUserFavoriteManager
{
    Task<UserFavorite> GetFavoriteByIdAsync(Guid favoriteId);
    Task<List<UserFavorite>> GetFavoriteByUserIdAsync(Guid userFavoriteId);
    Task<UserFavorite> AddFavoriteAsync(Guid userId, Guid trackId);
    Task<bool> DeleteFavoriteAsync(UserFavorite favorite);

    public Task<bool> FavoriteExistsAsync(Guid userId, Guid trackId);
}