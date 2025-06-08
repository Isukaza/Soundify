using Domain.Interfaces;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Models.DTO;
using Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;

namespace Soundify.DAL.PostgreSQL.Repository.Interfaces.db;

public interface ITrackRepository : IDbRepositoryBase<Track>
{
    IQueryable<Track> GetFilteredTracks(IFilter filter);

    Task<Track> GetTrackByIdAsync(Guid trackId);
    Task<Track> GetPublisherTrackByIdAsync(Guid publisherId, Guid trackId);
    Task<TrackUploadTokenData?> GetTrackUploadTokenDataAsync(Guid trackId);

    Task<bool> TrackExistsAsync(Guid trackId);
    Task<bool> IsTrackInAlbumOrSingleAsync(Guid trackId);
}