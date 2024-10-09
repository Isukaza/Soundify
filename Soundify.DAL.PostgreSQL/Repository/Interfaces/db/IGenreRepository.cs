using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;

namespace Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

public interface IGenreRepository : IDbRepositoryBase<Genre>
{
    Task<Genre> GetGenreByIdAsync(Guid id);

    Task<bool> HasRelatedRecords(Guid genreId);
    Task<bool> GenreExistsAsync(Guid genreId);
}