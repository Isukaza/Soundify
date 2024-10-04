using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Base;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

namespace Soundify.DAL.PostgreSQL.Repository.db;

public class GenreRepository : DbRepositoryBase<Genre>, IGenreRepository
{
    #region C-tor

    public GenreRepository(SoundifyDbContext dbContext) : base(dbContext)
    { }

    #endregion

    public async Task<Genre> GetGenreByIdAsync(Guid genreId) =>
        await DbContext.Genres.FirstOrDefaultAsync(g => g.Id == genreId);

    public async Task<bool> HasRelatedRecords(Guid genreId) =>
        await DbContext.Genres
            .AsNoTracking()
            .AnyAsync(g => g.Id == genreId && g.Tracks.Any());
}