using Domain.Interfaces;

namespace Soundify.DAL.PostgreSQL.Extensions;

public static class Extensions
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, IFilter filter) =>
        query.Skip((filter.Page - 1) * filter.Size).Take(filter.Size + 1);
}