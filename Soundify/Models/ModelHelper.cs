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