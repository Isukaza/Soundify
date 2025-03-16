using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using Domain.Interfaces;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Base;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

namespace Soundify.DAL.PostgreSQL.Repository.db;

public class TrackRepository : DbRepositoryBase<Track>, ITrackRepository
{
    #region C-tor

    public TrackRepository(SoundifyDbContext dbContext) : base(dbContext)
    { }

    #endregion

    #region Get

    public IQueryable<Track> GetFilteredTracks(ITrackFilter filter)
    {
        var query = DbContext.Tracks.AsNoTracking().AsQueryable();
        var filters = GetFilterExpressions(filter);
        return filters.Aggregate(query, (current, condition) => current.Where(condition));
    }

    public async Task<Track> GetTrackByIdAsync(Guid trackId) =>
        await DbContext.Tracks
            .Include(g => g.Genre)
            .FirstOrDefaultAsync(t => t.Id == trackId);

    public async Task<Track> GetPublisherTrackByIdAsync(Guid publisherId, Guid trackId) =>
        await DbContext.Tracks
            .FirstOrDefaultAsync(t => t.Id == trackId
                                      && ((t.Album != null && t.Album.Artist.PublisherId == publisherId)
                                          || (t.Single != null && t.Single.Artist.PublisherId == publisherId)));

    #endregion

    #region Checks

    public async Task<bool> TrackExistsAsync(Guid trackId) =>
        await DbContext.Tracks
            .AsNoTracking()
            .AnyAsync(t => t.Id == trackId);

    public async Task<bool> IsTrackInAlbumOrSingleAsync(Guid trackId) =>
        await DbContext.Tracks
            .AsNoTracking()
            .AnyAsync(t => t.Id == trackId && (t.Album != null || t.Single != null));

    #endregion
    
    #region Helpers
    
    private static List<Expression<Func<Track, bool>>> GetFilterExpressions(ITrackFilter filter)
    {
        var expressions = new List<Expression<Func<Track, bool>>>();

        if (filter.TrackId.HasValue)
            expressions.Add(t => t.Id == filter.TrackId.Value);

        if (!string.IsNullOrEmpty(filter.TrackName))
            expressions.Add(t => EF.Functions.ILike(t.Title, $"%{filter.TrackName}%"));

        if (filter.AlbumId.HasValue)
            expressions.Add(t => t.AlbumId == filter.AlbumId.Value);

        if (!string.IsNullOrEmpty(filter.AlbumName))
            expressions.Add(t => EF.Functions.ILike(t.Album.Title, $"%{filter.AlbumName}%"));

        if (filter.ArtistId.HasValue)
            expressions.Add(t => t.Album.ArtistId == filter.ArtistId.Value);

        if (!string.IsNullOrEmpty(filter.ArtistName))
            expressions.Add(t => EF.Functions.ILike(t.Album.Artist.Name, $"%{filter.ArtistName}%"));

        return expressions;
    }

    #endregion
}