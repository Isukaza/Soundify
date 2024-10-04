using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Create;

public class SingleCreateRequest
{
    [Required]
    public Guid ArtistId { get; init; }

    [Required]
    [StringLength(255)]
    public string CoverFilePath { get; init; }
    
    [Required]
    public TrackCreateRequest Track { get; init; }
}