namespace Soundify.Models.Response;

public class AlbumResponse
{
    public Guid Id { get; set; }
    public Guid ArtistId { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string CoverFilePath { get; set; }
}