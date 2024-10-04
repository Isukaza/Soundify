using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;

namespace Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

public interface IArtistRepository : IDbRepositoryBase<Artist>
{
    Task<Artist> GetArtistByIdAsync(Guid artistId);
    Task<bool> ArtistExists(Guid artistId);
}