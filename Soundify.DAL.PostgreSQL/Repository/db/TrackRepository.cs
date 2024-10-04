using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Base;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

namespace Soundify.DAL.PostgreSQL.Repository.db;

public class TrackRepository : DbRepositoryBase<Track>, ITrackRepository
{
    #region C-tor

    public TrackRepository(SoundifyDbContext dbContext) : base(dbContext)
    { }

    #endregion

    public async Task<Track> GetTrackByIdAsync(Guid trackId) =>
        await DbContext.Tracks
            .Include(g => g.Genre)
            .FirstOrDefaultAsync(t => t.Id == trackId);

    public async Task<bool> TrackExistsAsync(Guid trackId) =>
        await DbContext.Tracks
            .AsNoTracking()
            .AnyAsync(t => t.Id == trackId);
}