using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Base;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

namespace Soundify.DAL.PostgreSQL.Repository.db;

public class PlayListTrackRepository : DbRepositoryBase<PlayListTrack>, IPlayListTrackRepository
{
    #region C-tor

    public PlayListTrackRepository(SoundifyDbContext dbContext) : base(dbContext)
    { }

    #endregion

    public async Task<PlayListTrack> GetTrackByPlayListIdAsync(Guid playListId, Guid trackId) =>
        await DbContext.PlaylistTracks
            .FirstOrDefaultAsync(plt => plt.PlaylistId == playListId && plt.TrackId == trackId);

    public IQueryable<PlayListTrack> GetTracksByPlayListIdAsync(Guid playListId) =>
        DbContext.PlaylistTracks.Where(plt => plt.PlaylistId == playListId);

    public async Task<bool> PlayListTrackExistExistsAsync(Guid playListId, Guid trackId) =>
        await DbContext.PlaylistTracks
            .AsNoTracking()
            .AnyAsync(plt => plt.PlaylistId == playListId && plt.TrackId == trackId);
}