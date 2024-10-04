using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;

namespace Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

public interface ISingleRepository : IDbRepositoryBase<SingleTrack>
{
    Task<SingleTrack> GetSingleByIdAsync(Guid singleId);
}