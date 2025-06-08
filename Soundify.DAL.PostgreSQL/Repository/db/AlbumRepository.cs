using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Domain.Interfaces;
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

    #region Get

    public async Task<Album> GetAlbumByIdAsync(Guid albumId) =>
        await DbContext.Albums.FirstOrDefaultAsync(a => a.Id == albumId);

    public IQueryable<Album> GetFilteredAlbums(IFilter filter)
    {
        var query = DbContext.Albums.AsNoTracking().AsQueryable();
        var filters = GetFilterExpressions(filter);
        return filters.Aggregate(query, (current, condition) => current.Where(condition));
    }

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

    #endregion

    #region Helpers

    private static List<Expression<Func<Album, bool>>> GetFilterExpressions(IFilter filter)
    {
        var expressions = new List<Expression<Func<Album, bool>>>();

        if (filter.AlbumId.HasValue)
            expressions.Add(a => a.Id == filter.AlbumId.Value);

        if (!string.IsNullOrEmpty(filter.AlbumName))
            expressions.Add(a => EF.Functions.ILike(a.Title, $"%{filter.AlbumName}%"));

        if (filter.ArtistId.HasValue)
            expressions.Add(a => a.ArtistId == filter.ArtistId.Value);

        if (!string.IsNullOrEmpty(filter.ArtistName))
            expressions.Add(a => EF.Functions.ILike(a.Artist.Name, $"%{filter.ArtistName}%"));

        if (filter.TrackId.HasValue)
            expressions.Add(a => a.Tracks.Any(t => t.Id == filter.TrackId.Value));

        if (!string.IsNullOrEmpty(filter.TrackName))
            expressions.Add(a => a.Tracks.Any(t => EF.Functions.ILike(t.Title, $"%{filter.TrackName}%")));

        return expressions;
    }

    #endregion
}