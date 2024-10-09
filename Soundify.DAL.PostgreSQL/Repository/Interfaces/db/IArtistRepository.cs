using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;

namespace Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

public interface IArtistRepository : IDbRepositoryBase<Artist>
{
    Task<Artist> GetArtistByIdAsync(Guid artistId);
    Task<Artist> GetPublisherArtistByIdAsync(Guid publisherId, Guid artistId);

    Task<bool> ArtistExists(Guid artistId);
    Task<bool> IsPublisherOfArtistAsync(Guid publisherId, Guid artistId);
}