using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers.Interfaces;

public interface IPlayListManager
{
    Task<PlayList> GetPlayListByIdAsync(Guid playListId);
    Task<PlayListTrack> GetTrackByTrackIdAsync(Guid playListId, Guid trackId);
    Task<List<PlayListTrack>> GetTracksByPlayListIdAsync(Guid playListId);
    Task<PlayList> CreatePlayListAsync(PlayListCreateRequest playListData);
    Task<PlayListTrack> AddTrackToPlaylistAsync(PlayListAddTrackRequest trackData);
    Task<bool> UpdatePlayListAsync(PlayList playList, PlayListUpdateRequest playListData);
    Task<bool> DeletePlayListAsync(PlayList playList);
    Task<bool> RemoveTrackFromPlayList(PlayListTrack playListTrack);
    
    public Task<bool> PlayListExistExistsAsync(Guid playListId);
    public Task<bool> TrackExistExistsAsync(Guid playListId, Guid trackId);
}