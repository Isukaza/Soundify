using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Create;

public class AlbumCreateRequest
{
    [Required]
    [StringLength(100)]
    public string Title { get; init; }
    
    [Required]
    public Guid ArtistId { get; init; }

    [Required]
    public DateTime ReleaseDate { get; init; }
}