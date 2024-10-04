using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;
using Soundify.Managers.Interfaces;
using Soundify.Models.Request.Create;

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

    public async Task<UserFavorite> AddFavoriteAsync(UserFavoriteCreateRequest favoriteData)
    {
        if (favoriteData is null)
            return null;

        var userFavorite = new UserFavorite
        {
            UserId = favoriteData.UserId,
            TrackId = favoriteData.TrackId
        };

        return await _userFavoriteRepo.CreateAsync(userFavorite);
    }

    public async Task<bool> DeleteFavoriteAsync(UserFavorite favorite) =>
        favorite is not null && await _userFavoriteRepo.DeleteAsync(favorite);

    public Task<bool> FavoriteExistsAsync(Guid userId, Guid trackId) =>
        _userFavoriteRepo.FavoriteExistsAsync(userId, trackId);
}