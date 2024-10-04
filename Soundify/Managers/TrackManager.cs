using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;
using Soundify.Managers.Interfaces;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers;

public class TrackManager : ITrackManager
{
    private readonly ITrackRepository _trackRepo;

    public TrackManager(ITrackRepository trackRepo)
    {
        _trackRepo = trackRepo;
    }

    public async Task<Track> GetTrackByIdAsync(Guid trackId) =>
        await _trackRepo.GetTrackByIdAsync(trackId);

    public async Task<Track> CreateTrackAsync(TrackCreateRequest trackData, Genre genre)
    {
        if (trackData is null)
            return null;

        var track = new Track
        {
            Title = trackData.Title,
            Duration = 0,
            ReleaseDate = trackData.ReleaseDate,
            FilePath = string.Empty,
            GenreId = trackData.GenreId,
            Genre = genre
        };

        return await _trackRepo.CreateAsync(track);
    }

    public async Task<bool> UpdateTrackAsync(Track track, TrackUpdateRequest trackData)
    {
        if (track is null || trackData is null)
            return false;

        if (!string.IsNullOrEmpty(trackData.Title))
            track.Title = trackData.Title;

        if (trackData.ReleaseDate is not null)
            track.ReleaseDate = trackData.ReleaseDate.Value;

        return await _trackRepo.UpdateAsync(track);
    }

    public async Task<bool> DeleteTrackAsync(Track track) =>
        track is not null && await _trackRepo.DeleteAsync(track);

    public async Task<bool> TrackExistsAsync(Guid trackId) =>
        await _trackRepo.TrackExistsAsync(trackId);
}