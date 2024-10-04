using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Update;

public class TrackUpdateRequest
{
    [Required]
    public Guid Id { get; init; }
    
    [StringLength(100)]
    public  string Title { get; init; }

    public DateTime? ReleaseDate { get; init; }
}