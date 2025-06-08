using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Create;

public class TrackCreateRequest
{
    [Required]
    public Guid AlbumId { get; init; }
    
    [Required]
    public Guid GenreId { get; init; }
    
    [Required]
    [StringLength(100)]
    public string Title { get; init; }

    [Range(0, 1800)]
    public int Duration { get; init; }

    [Required]
    public DateTime ReleaseDate { get; init; }
}