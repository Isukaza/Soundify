using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;
using Soundify.Managers.Interfaces;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers;

public class ArtistSmManager : IArtistSmManager
{
    private readonly IArtistSmRepository _artistSmRepo;

    public ArtistSmManager(IArtistSmRepository artistSmRepo)
    {
        _artistSmRepo = artistSmRepo;
    }

    public async Task<ArtistSocialMedia> GetSocialMediaByIdAsync(Guid socialMediaId) =>
        await _artistSmRepo.GetSocialMediaByIdAsync(socialMediaId);

    public async Task<ArtistSocialMedia> GetPublisherSocialMediaByIdAsync(Guid publisherId, Guid socialMediaId) =>
        await _artistSmRepo.GetPublisherSocialMediaByIdAsync(publisherId, socialMediaId);

    public async Task<List<ArtistSocialMedia>> GetSocialMediaByArtistIdAsync(Guid artistId) =>
        await _artistSmRepo
            .GetSocialMediasByArtistId(artistId)
            .ToListAsync();

    public async Task<ArtistSocialMedia> AddSocialMediaAsync(ArtistSmCreateRequest smData)
    {
        if (smData is null)
            return null;

        var smLink = new ArtistSocialMedia
        {
            ArtistId = smData.ArtistId,
            Platform = smData.Platform,
            Url = smData.Url,
        };

        return await _artistSmRepo.CreateAsync(smLink);
    }

    public async Task<bool> UpdateSocialMediaAsync(ArtistSocialMedia artistSocialMedia, ArtistSmUpdateRequest asmData)
    {
        if (artistSocialMedia is null || asmData is null)
            return false;

        if (asmData.Platform is not null && asmData.Platform.Value != artistSocialMedia.Platform)
            artistSocialMedia.Platform = asmData.Platform.Value;

        if (!string.IsNullOrWhiteSpace(asmData.Url) && Uri.IsWellFormedUriString(asmData.Url, UriKind.Absolute))
            artistSocialMedia.Url = asmData.Url;

        return await _artistSmRepo.UpdateAsync(artistSocialMedia);
    }

    public async Task<bool> DeleteSocialMediaAsync(ArtistSocialMedia artistSocialMedia) =>
        artistSocialMedia is not null && await _artistSmRepo.DeleteAsync(artistSocialMedia);
}