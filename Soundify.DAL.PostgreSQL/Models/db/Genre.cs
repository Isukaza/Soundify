using System.ComponentModel.DataAnnotations;
using Soundify.DAL.PostgreSQL.Models.db.Base;

namespace Soundify.DAL.PostgreSQL.Models.db;

public record Genre : BaseDbEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    #region Relational

    public ICollection<Track> Tracks { get; set; } = new HashSet<Track>();
    
    #endregion
}