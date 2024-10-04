using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Response;

public class PlayListResponse
{
    [Required]
    public Guid Id { get; init; }

    [Required]
    [StringLength(100)]
    public string Title { get; init; }

    [StringLength(100)]
    public string Description { get; init; }
}