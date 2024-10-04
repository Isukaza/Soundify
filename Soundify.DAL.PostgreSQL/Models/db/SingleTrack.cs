using System.ComponentModel.DataAnnotations;
using Soundify.DAL.PostgreSQL.Models.db.Base;

namespace Soundify.DAL.PostgreSQL.Models.db;

public record SingleTrack : BaseDbEntity
{
    [Required]
    [StringLength(255)]
    public string CoverFilePath { get; set; }

    #region Relational

    public Guid TrackId { get; set; }
    public Track Track { get; set; }

    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }

    #endregion
}