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
}