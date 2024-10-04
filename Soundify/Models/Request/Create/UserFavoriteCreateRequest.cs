using System.ComponentModel.DataAnnotations;

namespace Soundify.Models.Request.Create;

public class UserFavoriteCreateRequest
{
    [Required]
    public Guid UserId { get; init; }

    [Required]
    public Guid TrackId { get; init; }
}