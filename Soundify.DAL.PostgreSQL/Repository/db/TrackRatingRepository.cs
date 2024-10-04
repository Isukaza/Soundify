using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Base;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

namespace Soundify.DAL.PostgreSQL.Repository.db;

public class TrackRatingRepository : DbRepositoryBase<TrackRating>, ITrackRatingRepository
{
    #region C-tor

    public TrackRatingRepository(SoundifyDbContext dbContext) : base(dbContext)
    { }

    #endregion

    public async Task<TrackRating> GetTrackRatingByIdAsync(Guid ratingId) =>
        await DbContext.TrackRatings.FirstOrDefaultAsync(tr => tr.Id == ratingId);

    public IQueryable<TrackRating> GetTrackRatingByTrackIdAsync(Guid trackId) =>
        DbContext.TrackRatings.Where(tr => tr.TrackId == trackId);

    public async Task<bool> TrackRatingExistsAsync(Guid userId, Guid trackId) =>
        await DbContext.TrackRatings
            .AsNoTracking()
            .AnyAsync(tr => tr.UserId == userId && tr.TrackId == trackId);
}