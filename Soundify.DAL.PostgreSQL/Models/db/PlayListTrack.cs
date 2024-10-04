using Soundify.DAL.PostgreSQL.Models.db.Base;

namespace Soundify.DAL.PostgreSQL.Models.db;

public record PlayListTrack : BaseDbEntity
{
    public Guid PlaylistId { get; set; }
    public PlayList PlayList { get; set; }

    public Guid TrackId { get; set; }
    public Track Track { get; set; }
}