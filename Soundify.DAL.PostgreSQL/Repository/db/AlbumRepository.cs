using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
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
}