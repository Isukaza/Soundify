using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;
using Soundify.Managers.Interfaces;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers;

public class SingleManager : ISingleManager
{
    private readonly ISingleRepository _singleRepo;

    public SingleManager(ISingleRepository singleRepo)
    {
        _singleRepo = singleRepo;
    }

    public async Task<SingleTrack> GetSingleTrack(Guid singleId) =>
        await _singleRepo.GetSingleByIdAsync(singleId);

    public async Task<SingleTrack> CreateSingleTrack(SingleCreateRequest trackData)
    {
        if (trackData is null || trackData.Track is null)
            return null;

        var track = new Track
        {
            Title = trackData.Track.Title,
            Duration = 542,
            ReleaseDate = trackData.Track.ReleaseDate,
            FilePath = string.Empty,
            GenreId = trackData.Track.GenreId
        };

        var single = new SingleTrack
        {
            ArtistId = trackData.ArtistId,
            TrackId = track.Id,
            Track = track,
            CoverFilePath = trackData.CoverFilePath
        };

        return await _singleRepo.CreateAsync(single);
    }

    public async Task<bool> UpdateSingleTrack(SingleTrack single, SingleUpdateRequest trackData)
    {
        if (single is null || single.Track is null || trackData is null)
            return false;

        if (!string.IsNullOrEmpty(trackData.Title) && single.Track.Title != trackData.Title)
            single.Track.Title = trackData.Title;

        if (trackData.ReleaseDate is not null && trackData.ReleaseDate.Value != trackData.ReleaseDate)
            single.Track.ReleaseDate = trackData.ReleaseDate.Value;

        if (!string.IsNullOrEmpty(trackData.CoverFilePath) && single.CoverFilePath != trackData.CoverFilePath)
            single.CoverFilePath = trackData.CoverFilePath;

        return await _singleRepo.UpdateAsync(single);
    }

    public async Task<bool> DeleteSingleTrack(SingleTrack track) =>
        track is not null && await _singleRepo.DeleteAsync(track);
}