using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;

namespace Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

public interface ITrackRepository : IDbRepositoryBase<Track>
{
    Task<Track> GetTrackByIdAsync(Guid trackId);
    Task<Track> GetPublisherTrackByIdAsync(Guid publisherId, Guid trackId);

    Task<bool> TrackExistsAsync(Guid trackId);
    Task<bool> IsTrackInAlbumOrSingleAsync(Guid trackId);
}