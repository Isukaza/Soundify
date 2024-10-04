using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Create;

public record ArtistCreateRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; init; }
}