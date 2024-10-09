using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers.Interfaces;

public interface IArtistSmManager
{
    Task<ArtistSocialMedia> GetSocialMediaByIdAsync(Guid socialMediaId);
    Task<ArtistSocialMedia> GetPublisherSocialMediaByIdAsync(Guid publisherId, Guid socialMediaId);
    Task<List<ArtistSocialMedia>> GetSocialMediaByArtistIdAsync(Guid artistId);

    Task<ArtistSocialMedia> AddSocialMediaAsync(ArtistSmCreateRequest smData);
    Task<bool> UpdateSocialMediaAsync(ArtistSocialMedia artistSocialMedia, ArtistSmUpdateRequest asmData);
    Task<bool> DeleteSocialMediaAsync(ArtistSocialMedia artistSocialMedia);
}