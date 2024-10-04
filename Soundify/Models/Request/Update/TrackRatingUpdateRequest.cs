using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Update;

public class TrackRatingUpdateRequest
{
    [Required]
    public Guid Id { get; init; }
    
    [Required]
    [Range(1, 5)]
    public double Rating { get; init; }
}