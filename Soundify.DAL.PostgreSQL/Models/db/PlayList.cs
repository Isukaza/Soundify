using System.ComponentModel.DataAnnotations;
using Soundify.DAL.PostgreSQL.Models.db.Base;

namespace Soundify.DAL.PostgreSQL.Models.db;

public record PlayList : BaseDbEntity
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [StringLength(100)]
    public string Description { get; set; }

    #region Relational

    public Guid UserId { get; set; }

    public ICollection<PlayListTrack> PlaylistTracks { get; set; } = new HashSet<PlayListTrack>();

    #endregion
}