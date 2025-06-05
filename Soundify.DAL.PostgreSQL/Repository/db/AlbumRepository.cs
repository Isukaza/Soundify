using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Models.DTO;
using Soundify.DAL.PostgreSQL.Repository.Base;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

namespace Soundify.DAL.PostgreSQL.Repository.db;

public class AlbumRepository : DbRepositoryBase<Album>, IAlbumRepository
{
    #region C-tor

    public AlbumRepository(SoundifyDbContext dbContext) : base(dbContext)
    { }

    #endregion

    public async Task<Album> GetAlbumByIdAsync(Guid albumId) =>
        await DbContext.Albums.FirstOrDefaultAsync(a => a.Id == albumId);

    public async Task<AlbumInfo> GetAlbumInfoByIdAsync(Guid albumId)
    {
        var album = await DbContext.Albums
            .Include(a => a.Artist)
            .FirstOrDefaultAsync(a => a.Id == albumId);

        if (album == null)
            return null;

        return new AlbumInfo
        {
            Id = album.Id,
            ArtistId = album.ArtistId,
            ArtistName = album.Artist?.Name,
            Title = album.Title,
            ReleaseDate = album.ReleaseDate,
            CoverFilePath = album.CoverFilePath
        };
    }

    public async Task<Album> GetPublisherAlbumByIdAsync(Guid publisherId, Guid albumId) =>
        await DbContext.Albums.FirstOrDefaultAsync(a => a.Id == albumId && a.Artist.PublisherId == publisherId);
}