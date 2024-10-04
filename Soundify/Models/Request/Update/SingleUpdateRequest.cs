using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Update;

public class SingleUpdateRequest
{
    [Required]
    public Guid Id { get; init; }
    
    [StringLength(100)]
    public  string Title { get; init; }

    public DateTime? ReleaseDate { get; init; }
    
    [StringLength(255)]
    public string CoverFilePath { get; init; }
}