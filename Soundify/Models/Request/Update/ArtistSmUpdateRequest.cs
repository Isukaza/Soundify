using System.ComponentModel.DataAnnotations;
using Soundify.DAL.PostgreSQL.Models.enums;

namespace Soundify.Models.Request.Update;

public class ArtistSmUpdateRequest
{
    [Required]
    public Guid Id { get; init; }
    
    [EnumDataType(typeof(SocialMediaPlatform))]
    public SocialMediaPlatform? Platform { get; init; }

    [StringLength(255)]
    [Url]
    public string Url { get; init; }
}