using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers.Interfaces;

public interface ITrackManager
{
    Task<Track> GetTrackByIdAsync(Guid trackId);
    Task<Track> CreateTrackAsync(TrackCreateRequest trackData, Genre genre);
    Task<bool> UpdateTrackAsync(Track track, TrackUpdateRequest updateRequest);
    Task<bool> DeleteTrackAsync(Track track);
    
    Task<bool> TrackExistsAsync(Guid trackId);
}