using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Base;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

namespace Soundify.DAL.PostgreSQL.Repository.db;

public class PlayListRepository : DbRepositoryBase<PlayList>, IPlayListRepository
{
    #region C-tor

    public PlayListRepository(SoundifyDbContext dbContext) : base(dbContext)
    { }

    #endregion

    public async Task<PlayList> GetPlayListByIdAsync(Guid playListId) =>
        await DbContext.Playlists.FirstOrDefaultAsync(pl => pl.Id == playListId);

    public async Task<bool> PlayListExistExistsAsync(Guid playListId) =>
        await DbContext.Playlists
            .AsNoTracking()
            .AnyAsync(pl => pl.Id == playListId);
}