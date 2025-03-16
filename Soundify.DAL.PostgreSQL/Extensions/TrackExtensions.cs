using Domain.Interfaces;
using Soundify.DAL.PostgreSQL.Models.db;

namespace Soundify.DAL.PostgreSQL.Extensions;

public static class TrackExtensions
{
    public static IQueryable<Track> ApplyPagination(
        this IQueryable<Track> query,
        ITrackFilter filter,
        out bool hasNextPage)
    {
        query = query.Skip((filter.Page - 1) * filter.Size).Take(filter.Size + 1);

        hasNextPage = query.Count() > filter.Size;
        
        return query.Take(filter.Size);
    }
}