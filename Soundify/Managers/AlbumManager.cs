using Microsoft.EntityFrameworkCore;

using Domain.Interfaces;
using Soundify.DAL.PostgreSQL.Extensions;
using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Models.DTO;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;
using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;
using Soundify.Models.Response;

namespace Soundify.Managers;

public class AlbumManager(IAlbumRepository albumRepo) : IAlbumManager
{
    public async Task<Album> GetAlbumByIdAsync(Guid albumId) =>
        await albumRepo.GetAlbumByIdAsync(albumId);

    public async Task<PagedAlbumsResult> GetAlbumsByFilterAsync(IFilter filter)
    {
        var query = albumRepo
            .GetFilteredAlbums(filter)
            .OrderBy(a => a.Title)
            .ApplyPagination(filter)
            .Select(album => new AlbumResponse
            {
                Id = album.Id,
                ArtistId = album.Artist.Id,
                ArtistName = album.Artist.Name,
                Title = album.Title,
                ReleaseDate = album.ReleaseDate,
                CoverFilePath = album.CoverFilePath
            });

        var albums = await query.ToListAsync();
        var hasNextPage = albums.Count > filter.Size;
        if (hasNextPage)
            albums.RemoveAt(albums.Count - 1);

        return new PagedAlbumsResult
        {
            Albums = albums,
            HasNextPage = hasNextPage
        };
    }

    public async Task<AlbumResponse> GetAlbumInfoByIdAsync(Guid albumId) =>
        await albumRepo
            .GetAlbumInfoById(albumId)
            .Select(album => new AlbumResponse
            {
                Id = album.Id,
                ArtistId = album.Artist.Id,
                ArtistName = album.Artist.Name,
                Title = album.Title,
                ReleaseDate = album.ReleaseDate,
                CoverFilePath = album.CoverFilePath
            })
            .FirstOrDefaultAsync();

    public async Task<Album> GetPublisherAlbumByIdAsync(Guid publisherId, Guid albumId) =>
        await albumRepo.GetPublisherAlbumByIdAsync(publisherId, albumId);

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

        return await albumRepo.CreateAsync(album);
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

        return await albumRepo.UpdateAsync(album);
    }

    public async Task<bool> DeleteAlbumAsync(Album album) =>
        album is not null && await albumRepo.DeleteAsync(album);
}