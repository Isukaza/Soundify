using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Create;

public record ArtistCreateRequest
{
    [Required]
    public Guid PublisherId { get; init; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; init; }
}