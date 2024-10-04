using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers.Interfaces;

public interface ITrackRatingManager
{
    Task<TrackRating> GetTrackRatingByIdAsync(Guid ratingId);
    Task<List<TrackRating>> GetTrackRatingByTrackIdAsync(Guid trackId);
    Task<TrackRating> AddTrackRatingAsync(TrackRatingCreateRequest trackRatingData);
    Task<bool> UpdateTrackRatingAsync(TrackRating trackRating, TrackRatingUpdateRequest trackRatingData);
    Task<bool> DeleteTrackRatingAsync(TrackRating trackRating);

    Task<bool> TrackRatingExistsAsync(Guid userId, Guid trackId);
}