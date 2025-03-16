namespace Soundify.Models.Response;

public class TrackResponse
{
    public Guid TrackId { get; set; }
    public string TrackName { get; set; }
    
    public Guid AlbumId { get; set; }
    public string AlbumName { get; set; }
    
    public Guid ArtistId { get; set; }
    public string ArtistName { get; set; }
    
    public int Duration { get; set; }
    public double TotalRating { get; set; }
    public string FilePath { get; set; }
    public string Genre { get; set; }
}