using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Base;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

namespace Soundify.DAL.PostgreSQL.Repository.db;

public class ArtistRepository : DbRepositoryBase<Artist>, IArtistRepository
{
    #region C-tor

    public ArtistRepository(SoundifyDbContext dbContext) : base(dbContext)
    { }

    #endregion

    public async Task<Artist> GetArtistByIdAsync(Guid artistId) =>
        await DbContext.Artists.FirstOrDefaultAsync(a => a.Id == artistId);

    public async Task<bool> ArtistExists(Guid artistId) =>
        await DbContext.Artists.AnyAsync(a => a.Id == artistId);
}