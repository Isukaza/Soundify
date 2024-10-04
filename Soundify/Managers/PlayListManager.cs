using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;
using Soundify.Managers.Interfaces;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers;

public class PlayListManager : IPlayListManager
{
    private readonly IPlayListRepository _playListRepo;
    private readonly IPlayListTrackRepository _playListTrackRepo;

    public PlayListManager(IPlayListRepository playListRepo, IPlayListTrackRepository playListTrackRepo)
    {
        _playListRepo = playListRepo;
        _playListTrackRepo = playListTrackRepo;
    }

    public async Task<PlayList> GetPlayListByIdAsync(Guid playListId) =>
        await _playListRepo.GetPlayListByIdAsync(playListId);

    public async Task<PlayListTrack> GetTrackByTrackIdAsync(Guid playListId, Guid trackId) =>
        await _playListTrackRepo.GetTrackByPlayListIdAsync(playListId, trackId);

    public async Task<List<PlayListTrack>> GetTracksByPlayListIdAsync(Guid playListId) =>
        await _playListTrackRepo
            .GetTracksByPlayListIdAsync(playListId)
            .ToListAsync();

    public async Task<PlayList> CreatePlayListAsync(PlayListCreateRequest playListData)
    {
        if (playListData is null)
            return null;

        var playList = new PlayList
        {
            UserId = playListData.UserId,
            Title = playListData.Title,
            Description = playListData.Description
        };

        return await _playListRepo.CreateAsync(playList);
    }

    public async Task<PlayListTrack> AddTrackToPlaylistAsync(PlayListAddTrackRequest trackData)
    {
        if (trackData is null)
            return null;

        var playListTrack = new PlayListTrack
        {
            PlaylistId = trackData.PlayListId,
            TrackId = trackData.TrackId,
        };

        return await _playListTrackRepo.CreateAsync(playListTrack);
    }

    public async Task<bool> UpdatePlayListAsync(PlayList playList, PlayListUpdateRequest playListData)
    {
        if (playList is null || playListData is null)
            return false;

        if (!string.IsNullOrEmpty(playListData.Title) && playList.Title != playListData.Title)
            playList.Title = playListData.Title;

        if (!string.IsNullOrEmpty(playListData.Description) && playList.Description != playListData.Description)
            playList.Description = playListData.Description;

        return await _playListRepo.UpdateAsync(playList);
    }

    public async Task<bool> DeletePlayListAsync(PlayList playList) =>
        playList is not null && await _playListRepo.DeleteAsync(playList);

    public async Task<bool> RemoveTrackFromPlayList(PlayListTrack playListTrack) =>
        playListTrack is not null && await _playListTrackRepo.DeleteAsync(playListTrack);

    public async Task<bool> PlayListExistExistsAsync(Guid playListId) =>
        await _playListRepo.PlayListExistExistsAsync(playListId);

    public async Task<bool> TrackExistExistsAsync(Guid playListId, Guid trakId) =>
        await _playListTrackRepo.PlayListTrackExistExistsAsync(playListId, trakId);
}