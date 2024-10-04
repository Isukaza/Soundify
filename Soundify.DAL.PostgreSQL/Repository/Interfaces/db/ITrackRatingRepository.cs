using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;

namespace Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

public interface ITrackRatingRepository : IDbRepositoryBase<TrackRating>
{
    Task<TrackRating> GetTrackRatingByIdAsync(Guid ratingId);
    IQueryable<TrackRating> GetTrackRatingByTrackIdAsync(Guid trackId);
    
    Task<bool> TrackRatingExistsAsync(Guid userId, Guid trackId);
}