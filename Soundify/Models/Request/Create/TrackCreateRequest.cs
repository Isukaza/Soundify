using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Create;

public class TrackCreateRequest
{
    [Required]
    [StringLength(100)]
    public string Title { get; init; }

    [Required]
    public Guid GenreId { get; init; }

    [Required]
    public DateTime ReleaseDate { get; init; }
}