using Soundify.DAL.PostgreSQL.Models.enums;

namespace Soundify.Models.Response;

public class ArtistSmResponse
{
    public Guid Id { get; set; }
    public Guid ArtistId { get; set; }
    public SocialMediaPlatform Platform { get; set; }
    public string Url { get; set; }
}