using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Create;

public class TrackRatingCreateRequest
{
    [Required]
    public Guid TrackId { get; init; }
    
    [Required]
    [Range(1, 5)]
    public double Rating { get; init; }
}