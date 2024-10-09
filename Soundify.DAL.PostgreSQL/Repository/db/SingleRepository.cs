using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Base;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

namespace Soundify.DAL.PostgreSQL.Repository.db;

public class SingleRepository : DbRepositoryBase<SingleTrack>, ISingleRepository
{
    #region C-tor

    public SingleRepository(SoundifyDbContext dbContext) : base(dbContext)
    { }

    #endregion

    public async Task<SingleTrack> GetSingleByIdAsync(Guid singleId) =>
        await DbContext.SingleTracks
            .Include(t => t.Track)
            .FirstOrDefaultAsync(st => st.Id == singleId);

    public async Task<SingleTrack> GetPublisherSingleByIdAsync(Guid publisherId, Guid singleId) =>
        await DbContext.SingleTracks
            .FirstOrDefaultAsync(st => st.Id == singleId && st.Artist.PublisherId == publisherId);
}