using Soundify.Models.Response;

namespace Soundify.Models;

public class PagedTracksResult
{
    public List<TrackResponse> Tracks { get; init; }
    public bool HasNextPage { get; init; }
}