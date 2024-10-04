namespace Soundify.Models.Response;

public class SingleResponse
{
    public Guid Id { get; set; }
    public Guid TrackId { get; set; }
    public Guid ArtistId { get; set; }
    public string CoverFilePath { get; set; }
    public string Title { get; set; }
    public int Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string FilePath { get; set; }
    public double Rating { get; set; }
}