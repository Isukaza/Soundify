using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Update;

public class GenreUpdateRequest
{
    [Required]
    public Guid Id { get; init; }

    [Required]
    public string Name { get; init; }
}