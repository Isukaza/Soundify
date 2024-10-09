using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers.Interfaces;

public interface IArtistManager
{
    Task<Artist> GetArtistByIdAsync(Guid artistId);
    Task<Artist> GetPublisherArtistByIdAsync(Guid publisherId, Guid artistId);
    
    Task<Artist> CreateArtistAsync(ArtistCreateRequest artistData);
    Task<bool> UpdateArtistAsync(Artist artist, ArtistUpdateRequest artistData);
    Task<bool> DeleteArtistAsync(Artist artist);
    
    Task<bool> ArtistExistsAsync(Guid artistId);
    Task<bool> IsPublisherOfArtistAsync(Guid publisherId, Guid artistId);
}