namespace Soundify.Models.Response;

public class UserFavoriteResponse
{
    public Guid UserId { get; set; }
    public Guid TrackId { get; set; }
}