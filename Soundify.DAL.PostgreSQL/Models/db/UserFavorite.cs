using Soundify.DAL.PostgreSQL.Models.db.Base;

namespace Soundify.DAL.PostgreSQL.Models.db;

public record UserFavorite : BaseDbEntity
{
    public Guid UserId { get; set; }
    
    public Guid TrackId { get; set; }
    public Track Track { get; set; }
}