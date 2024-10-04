using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;

namespace Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

public interface IPlayListTrackRepository : IDbRepositoryBase<PlayListTrack>
{
    Task<PlayListTrack> GetTrackByPlayListIdAsync(Guid playListId, Guid trackId);
    IQueryable<PlayListTrack> GetTracksByPlayListIdAsync(Guid playListId);
    
    Task<bool> PlayListTrackExistExistsAsync(Guid playListId, Guid trackId);
}