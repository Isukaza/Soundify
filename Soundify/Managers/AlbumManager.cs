using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;
using Soundify.Managers.Interfaces;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers;

public class AlbumManager : IAlbumManager
{
    private readonly IAlbumRepository _albumRepo;

    public AlbumManager(IAlbumRepository albumRepo)
    {
        _albumRepo = albumRepo;
    }

    public async Task<Album> GetAlbumByIdAsync(Guid albumId) =>
        await _albumRepo.GetAlbumByIdAsync(albumId);

    public async Task<Album> GetPublisherAlbumByIdAsync(Guid publisherId, Guid albumId) =>
        await _albumRepo.GetPublisherAlbumByIdAsync(publisherId, albumId);

    public async Task<Album> CreateAlbumAsync(AlbumCreateRequest albumData)
    {
        if (albumData is null)
            return null;

        var album = new Album
        {
            ArtistId = albumData.ArtistId,
            Title = albumData.Title,
            ReleaseDate = albumData.ReleaseDate,
            CoverFilePath = string.Empty,
        };

        return await _albumRepo.CreateAsync(album);
    }

    public async Task<bool> UpdateAlbumAsync(Album album, AlbumUpdateRequest albumData)
    {
        if (album is null || albumData is null)
            return false;

        if (!string.IsNullOrWhiteSpace(albumData.Title))
            album.Title = albumData.Title.Trim();

        if (albumData.ReleaseDate != null)
            album.ReleaseDate = albumData.ReleaseDate.Value;

        if (!string.IsNullOrEmpty(albumData.CoverFilePath))
            album.CoverFilePath = albumData.CoverFilePath.Trim();

        return await _albumRepo.UpdateAsync(album);
    }

    public async Task<bool> DeleteAlbumAsync(Album album) =>
        album is not null && await _albumRepo.DeleteAsync(album);
}