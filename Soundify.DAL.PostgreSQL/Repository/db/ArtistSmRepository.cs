using Microsoft.EntityFrameworkCore;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Base;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

namespace Soundify.DAL.PostgreSQL.Repository.db;

public class ArtistSmRepository : DbRepositoryBase<ArtistSocialMedia>, IArtistSmRepository
{
    #region C-tor

    public ArtistSmRepository(SoundifyDbContext dbContext) : base(dbContext)
    { }

    #endregion

    public async Task<ArtistSocialMedia> GetSocialMediaByIdAsync(Guid socialMediaId) =>
        await DbContext.ArtistSocialMedias.FirstOrDefaultAsync(asm => asm.Id == socialMediaId);

    public async Task<ArtistSocialMedia> GetPublisherSocialMediaByIdAsync(Guid publisherId, Guid socialMediaId) =>
        await DbContext.ArtistSocialMedias
            .FirstOrDefaultAsync(asm => asm.Id == socialMediaId && asm.Artist.PublisherId == publisherId);

    public IQueryable<ArtistSocialMedia> GetSocialMediasByArtistId(Guid artistId) =>
        DbContext.ArtistSocialMedias.Where(asm => asm.ArtistId == artistId);
}