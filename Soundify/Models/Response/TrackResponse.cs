namespace Soundify.Models.Response;

public class TrackResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string FilePath { get; set; }
    public int Duration { get; set; }
    public double Rating { get; set; }
}