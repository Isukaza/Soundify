using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Create;

public class PlayListCreateRequest
{
    [Required]
    [StringLength(100)]
    public string Title { get; init; }
    
    [Required]
    [StringLength(100)]
    public string Description { get; init; }
}