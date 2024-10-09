using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;
using Soundify.Managers.Interfaces;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers;

public class TrackRatingManager : ITrackRatingManager
{
    private readonly ITrackRatingRepository _trackRatingRepo;

    public TrackRatingManager(ITrackRatingRepository trackRatingRepo)
    {
        _trackRatingRepo = trackRatingRepo;
    }

    public async Task<TrackRating> GetTrackRatingByIdAsync(Guid ratingId) =>
        await _trackRatingRepo.GetTrackRatingByIdAsync(ratingId);

    public async Task<List<TrackRating>> GetTrackRatingByTrackIdAsync(Guid trackId) =>
        await _trackRatingRepo.GetTrackRatingByTrackIdAsync(trackId).ToListAsync();

    public async Task<TrackRating> AddTrackRatingAsync(Guid userId, TrackRatingCreateRequest trackRatingData)
    {
        if (trackRatingData is null)
            return null;

        var trackRating = new TrackRating
        {
            UserId = userId,
            TrackId = trackRatingData.TrackId,
            Rating = trackRatingData.Rating
        };

        return await _trackRatingRepo.CreateAsync(trackRating);
    }

    public async Task<bool> UpdateTrackRatingAsync(TrackRating trackRating, TrackRatingUpdateRequest trackRatingData)
    {
        if (trackRating is null || trackRatingData is null)
            return false;

        trackRating.Rating = trackRatingData.Rating;
        return await _trackRatingRepo.UpdateAsync(trackRating);
    }

    public async Task<bool> DeleteTrackRatingAsync(TrackRating trackRating) =>
        trackRating is not null && await _trackRatingRepo.DeleteAsync(trackRating);

    public async Task<bool> TrackRatingExistsAsync(Guid userId, Guid trackId) =>
        await _trackRatingRepo.TrackRatingExistsAsync(userId, trackId);
}