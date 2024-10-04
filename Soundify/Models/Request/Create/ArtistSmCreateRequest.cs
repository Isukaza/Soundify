using System.ComponentModel.DataAnnotations;
using Soundify.DAL.PostgreSQL.Models.enums;

namespace Soundify.Models.Request.Create;

public class ArtistSmCreateRequest
{
    [Required]
    public Guid ArtistId { get; init; }

    [Required]
    [EnumDataType(typeof(SocialMediaPlatform))]
    public SocialMediaPlatform Platform { get; init; }

    [Required]
    [StringLength(255)] [Url] public string Url { get; init; }
}