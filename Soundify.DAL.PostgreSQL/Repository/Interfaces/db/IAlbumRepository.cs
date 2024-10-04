using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;

namespace Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

public interface IAlbumRepository : IDbRepositoryBase<Album>
{
    Task<Album> GetAlbumByIdAsync(Guid albumId);
}