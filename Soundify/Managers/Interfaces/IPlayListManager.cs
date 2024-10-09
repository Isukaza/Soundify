using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers.Interfaces;

public interface IPlayListManager
{
    Task<PlayList> GetPlayListByIdAsync(Guid playListId);
    Task<Guid?> GetPlaylistOwnerId(Guid playlistId);
    Task<PlayListTrack> GetTrackByTrackIdAsync(Guid playListId, Guid trackId);
    Task<List<PlayListTrack>> GetTracksByPlayListIdAsync(Guid playListId);
    Task<List<PlayListTrack>> GetUserPlaylistTracksAsync(Guid userId, Guid playListId);

    Task<PlayList> CreatePlayListAsync(Guid userId, PlayListCreateRequest playListData);
    Task<PlayListTrack> AddTrackToPlaylistAsync(PlayListAddTrackRequest trackData);

    Task<bool> UpdatePlayListAsync(PlayList playList, PlayListUpdateRequest playListData);

    Task<bool> DeletePlayListAsync(PlayList playList);
    Task<bool> RemoveTrackFromPlayList(PlayListTrack playListTrack);

    public Task<bool> TrackExistsAsync(Guid playListId, Guid trackId);
}