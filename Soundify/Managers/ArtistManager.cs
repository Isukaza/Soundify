using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;
using Soundify.Managers.Interfaces;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers;

public class ArtistManager : IArtistManager
{
    private readonly IArtistRepository _artistRepo;

    public ArtistManager(IArtistRepository artistRepo)
    {
        _artistRepo = artistRepo;
    }

    public Task<Artist> GetArtistByIdAsync(Guid artistId) =>
        _artistRepo.GetArtistByIdAsync(artistId);

    public async Task<Artist> CreateArtistAsync(ArtistCreateRequest artistData)
    {
        if (artistData is null)
            return null;

        var artist = new Artist
        {
            Name = artistData.Name,
            ImageFilePath = string.Empty
        };

        return await _artistRepo.CreateAsync(artist);
    }

    public async Task<bool> UpdateArtistAsync(Artist artist, ArtistUpdateRequest artistData)
    {
        if (artistData is null || artist is null)
            return false;

        if (!string.IsNullOrEmpty(artistData.Name))
            artist.Name = artistData.Name;

        if (!string.IsNullOrEmpty(artistData.ImageFilePath))
            artist.ImageFilePath = artistData.ImageFilePath;

        return await _artistRepo.UpdateAsync(artist);
    }

    public async Task<bool> DeleteArtistAsync(Artist artist) =>
        artist is not null && await _artistRepo.DeleteAsync(artist);

    public async Task<bool> ArtistExistsAsync(Guid artistId) =>
        await _artistRepo.ArtistExists(artistId);
}