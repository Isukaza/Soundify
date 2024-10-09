using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers.Interfaces;

public interface IGenreManager
{
    Task<Genre> GetGenreByIdAsync(Guid genreId);
    Task<Genre> CreateGenreAsync(GenreCreateRequest genreData);
    Task<bool> UpdateGenreAsync(Genre genre, GenreUpdateRequest genreData);
    Task<bool> DeleteGenreAsync(Genre genre);

    Task<bool> HasRelatedRecordsAsync(Guid genreId);
    Task<bool> GenreExistsAsync(Guid genreId);
}