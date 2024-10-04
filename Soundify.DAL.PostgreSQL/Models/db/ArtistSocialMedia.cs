using System.ComponentModel.DataAnnotations;
using Soundify.DAL.PostgreSQL.Models.db.Base;
using Soundify.DAL.PostgreSQL.Models.enums;

namespace Soundify.DAL.PostgreSQL.Models.db;

public record ArtistSocialMedia : BaseDbEntity
{
    [Required]
    [EnumDataType(typeof(SocialMediaPlatform))]
    public SocialMediaPlatform Platform { get; set; }

    [Required]
    [StringLength(255)]
    public string Url { get; set; }

    #region Relational
    
    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }

    #endregion
}
