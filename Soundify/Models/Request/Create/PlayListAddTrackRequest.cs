using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Create;

public class PlayListAddTrackRequest
{
    [Required]
    public Guid PlayListId { get; set; }
    
    [Required]
    public Guid TrackId { get; set; }
}