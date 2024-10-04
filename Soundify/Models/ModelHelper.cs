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

    public static AlbumResponse ToAlbumResponse(this Album album) =>
        new()
        {
            Id = album.Id,
            ArtistId = album.ArtistId,
            Title = album.Title,
            ReleaseDate = album.ReleaseDate,
            CoverFilePath = album.CoverFilePath
        };

    public static TrackResponse ToTrackResponse(this Track track)
    {
        var trackResponse = new TrackResponse
        {
            Id = track.Id,
            Title = track.Title,
            Genre = track.Genre.Name,
            FilePath = track.FilePath,
            Duration = track.Duration,
        };

        if (track.TotalRating > 0 && track.RatingCount > 0)
            trackResponse.Rating = Math.Round(track.TotalRating / track.RatingCount, 2);

        return trackResponse;
    }

    public static GenreResponse ToGenreResponse(this Genre genre) =>
        new()
        {
            Id = genre.Id,
            Name = genre.Name
        };

    public static ArtistSmResponse ToArtistSmResponse(this ArtistSocialMedia artistSm) =>
        new()
        {
            Id = artistSm.Id,
            ArtistId = artistSm.ArtistId,
            Platform = artistSm.Platform,
            Url = artistSm.Url
        };
}