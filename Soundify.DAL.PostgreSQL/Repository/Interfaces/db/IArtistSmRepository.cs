using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;

namespace Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

public interface IArtistSmRepository : IDbRepositoryBase<ArtistSocialMedia>
{
    Task<ArtistSocialMedia> GetSocialMediaByIdAsync(Guid socialMediaId);
    Task<ArtistSocialMedia> GetPublisherSocialMediaByIdAsync(Guid publisherId, Guid socialMediaId);
    IQueryable<ArtistSocialMedia> GetSocialMediasByArtistId(Guid artistId);
}