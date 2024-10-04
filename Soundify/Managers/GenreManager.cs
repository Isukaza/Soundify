using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;
using Soundify.Managers.Interfaces;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers;

public class GenreManager : IGenreManager
{
    private readonly IGenreRepository _genreRepo;

    public GenreManager(IGenreRepository genreRepo)
    {
        _genreRepo = genreRepo;
    }

    public async Task<Genre> GetGenreByIdAsync(Guid genreId) =>
        await _genreRepo.GetGenreByIdAsync(genreId);

    public async Task<Genre> CreateGenreAsync(GenreCreateRequest genreData)
    {
        if (genreData is null)
            return null;

        var genre = new Genre
        {
            Name = genreData.Name
        };

        return await _genreRepo.CreateAsync(genre);
    }

    public async Task<bool> UpdateGenreAsync(Genre genre, GenreUpdateRequest genreData)
    {
        if (genre is null || genreData is null)
            return false;

        if (!string.IsNullOrEmpty(genre.Name) && genre.Name != genreData.Name)
        {
            genre.Name = genreData.Name;
            return await _genreRepo.UpdateAsync(genre);
        }

        return true;
    }

    public async Task<bool> DeleteGenreAsync(Genre genre) =>
        genre is not null && await _genreRepo.DeleteAsync(genre);

    public async Task<bool> HasRelatedRecordsAsync(Guid genreId) =>
        await _genreRepo.HasRelatedRecords(genreId);
}