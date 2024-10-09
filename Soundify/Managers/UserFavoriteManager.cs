using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;
using Soundify.Managers.Interfaces;

namespace Soundify.Managers;

public class UserFavoriteManager : IUserFavoriteManager
{
    private readonly IUserFavoriteRepository _userFavoriteRepo;

    public UserFavoriteManager(IUserFavoriteRepository userFavoriteRepo)
    {
        _userFavoriteRepo = userFavoriteRepo;
    }

    public async Task<UserFavorite> GetFavoriteByIdAsync(Guid favoriteId) =>
        await _userFavoriteRepo.GetFavoriteByIdAsync(favoriteId);

    public async Task<List<UserFavorite>> GetFavoriteByUserIdAsync(Guid userFavoriteId) =>
        await _userFavoriteRepo
            .GetUserFavoriteByUserIdAsync(userFavoriteId)
            .ToListAsync();

    public async Task<UserFavorite> AddFavoriteAsync(Guid userId, Guid trackId)
    {
        var userFavorite = new UserFavorite
        {
            UserId = userId,
            TrackId = trackId
        };

        return await _userFavoriteRepo.CreateAsync(userFavorite);
    }

    public async Task<bool> DeleteFavoriteAsync(UserFavorite favorite) =>
        favorite is not null && await _userFavoriteRepo.DeleteAsync(favorite);

    public async Task<bool> FavoriteExistsAsync(Guid userId, Guid trackId) =>
        await _userFavoriteRepo.FavoriteExistsAsync(userId, trackId);
}