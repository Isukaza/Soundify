using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Update;

public class ArtistUpdateRequest
{
    [Required]
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string ImageFilePath { get; init; }
}