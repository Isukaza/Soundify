namespace Domain.Interfaces;

public interface IFilter : IPagination
{
    public Guid? TrackId { get; init; }

    public string TrackName { get; init; }

    public Guid? AlbumId { get; init; }

    public string AlbumName { get; init; }

    public Guid? ArtistId { get; init; }

    public string ArtistName { get; init; }
}