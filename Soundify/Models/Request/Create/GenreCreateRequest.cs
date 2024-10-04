using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Create;

public class GenreCreateRequest
{
    [Required]
    public string Name { get; init; }
}