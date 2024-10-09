using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;

namespace Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

public interface IPlayListRepository : IDbRepositoryBase<PlayList>
{
    Task<PlayList> GetPlayListByIdAsync(Guid playListId);
    Task<Guid?> GetPlaylistOwnerId(Guid playlistId);
}