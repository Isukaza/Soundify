using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers.Interfaces;

public interface IAlbumManager
{
    Task<Album> GetAlbumByIdAsync(Guid albumId);
    Task<Album> GetPublisherAlbumByIdAsync(Guid publisherId, Guid albumId);

    Task<Album> CreateAlbumAsync(AlbumCreateRequest albumData);
    Task<bool> UpdateAlbumAsync(Album album, AlbumUpdateRequest albumData);
    Task<bool> DeleteAlbumAsync(Album album);
}