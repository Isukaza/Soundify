using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Update;

public class PlayListUpdateRequest
{
    [Required]
    public Guid Id { get; init; }

    [StringLength(100)]
    public string Title { get; init; }

    [StringLength(100)]
    public string Description { get; init; }
}