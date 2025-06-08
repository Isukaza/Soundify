using Soundify.Models.Response;

namespace Soundify.Models;

public class PagedAlbumsResult
{
    public List<AlbumResponse> Albums { get; init; }
    public bool HasNextPage { get; init; }
}