using System.ComponentModel.DataAnnotations;
using Soundify.DAL.PostgreSQL.Models.db.Base;

namespace Soundify.DAL.PostgreSQL.Models.db;

public record Artist : BaseDbEntity
{
    [Required]
    public Guid PublisherId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(255)]
    public string ImageFilePath { get; set; }

    #region Relational

    public ICollection<SingleTrack> Singles { get; set; } = new HashSet<SingleTrack>();
    public ICollection<Album> Albums { get; set; } = new HashSet<Album>();
    public ICollection<ArtistSocialMedia> SocialMediaLinks { get; set; } = new HashSet<ArtistSocialMedia>();   
    
    #endregion
}