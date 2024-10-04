using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.Models.Response;

namespace Soundify.Models;

public static class ModelHelper
{
    public static ArtistResponse ToArtistResponse(this Artist artist) =>
        new()
        {
            Id = artist.Id,
            Name = artist.Name,
            ImageFilePath = artist.ImageFilePath
        };
}