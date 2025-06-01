namespace Soundify.DAL.PostgreSQL.Models.DTO;

public class TrackUploadTokenData
{
    public Guid ArtistId { get; init; }
    public Guid? AlbumId { get; init; }
    public Guid PublisherId { get; init; }
}