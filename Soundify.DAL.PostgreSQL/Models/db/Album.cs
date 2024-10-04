using System.ComponentModel.DataAnnotations;
using Soundify.DAL.PostgreSQL.Models.db.Base;

namespace Soundify.DAL.PostgreSQL.Models.db;

public record Album : BaseDbEntity
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    [Required]
    [StringLength(255)]
    public string CoverFilePath { get; set; }

    #region Relational

    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }
    
    public ICollection<Track> Tracks { get; set; } = new HashSet<Track>();

    #endregion
}