using Microsoft.EntityFrameworkCore;

using Domain.Interfaces;

using Soundify.DAL.PostgreSQL.Extensions;
using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;
using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;
using Soundify.Models.Response;

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

    public async Task<PagedTracksResult> GetTracksByFilterAsync(ITrackFilter filter)
    {
        var query = _trackRepo
            .GetFilteredTracks(filter)
            .OrderBy(t => t.Title)
            .ApplyPagination(filter)
            .Select(track => new TrackResponse
            {
                TrackId = track.Id,
                TrackName = track.Title,
                FilePath = track.FilePath,
                Duration = track.Duration,
                Genre = track.Genre.Name,
                AlbumId = track.AlbumId ?? Guid.Empty,
                AlbumName = track.Album != null ? track.Album.Title : string.Empty,
                ArtistId = track.Album != null && track.Album.Artist != null ? track.Album.Artist.Id : Guid.Empty,
                ArtistName = track.Album != null && track.Album.Artist != null ? track.Album.Artist.Name : string.Empty,
                TotalRating = track.RatingCount > 0 ? track.TotalRating / track.RatingCount : 0
            });

        var tracks = await query.ToListAsync();

        var hasNextPage = tracks.Count > filter.Size;
        if (hasNextPage)
            tracks.RemoveAt(tracks.Count - 1);

        foreach (var track in tracks)
            track.TotalRating = Math.Round(track.TotalRating, 2);

        return new PagedTracksResult
        {
            Tracks = tracks,
            HasNextPage = hasNextPage
        };
    }

    public async Task<Track> GetPublisherTrackByIdAsync(Guid publisherId, Guid trackId) =>
        await _trackRepo.GetPublisherTrackByIdAsync(publisherId, trackId);

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

    public async Task<bool> IsTrackInAlbumOrSingleAsync(Guid trackId) =>
        await _trackRepo.IsTrackInAlbumOrSingleAsync(trackId);
}