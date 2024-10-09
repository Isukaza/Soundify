using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.Models.Response;

namespace Soundify.Models;

public static class ModelHelper
{
    public static ArtistResponse ToArtistResponse(this Artist artist) =>
        new()
        {
            Id = artist.Id,
            PublisherId = artist.PublisherId,
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

    public static UserFavoriteResponse ToUserFavoriteResponse(this UserFavorite favorite) =>
        new()
        {
            UserId = favorite.UserId,
            TrackId = favorite.TrackId
        };

    public static TrackRatingResponse ToTrackRatingResponse(this TrackRating trackRating) =>
        new()
        {
            Id = trackRating.Id,
            Rating = trackRating.Rating
        };

    public static SingleResponse ToSingleResponse(this SingleTrack single)
    {
        var singleResponse = new SingleResponse
        {
            Id = single.Id,
            TrackId = single.TrackId,
            ArtistId = single.ArtistId,
            CoverFilePath = single.CoverFilePath,
            Title = single.Track?.Title ?? string.Empty,
            Duration = single.Track?.Duration ?? 0,
            ReleaseDate = single.Track?.ReleaseDate ?? DateTime.MinValue,
            FilePath = single.Track?.FilePath ?? string.Empty,
        };
        
        if (single.Track is not null && single.Track.RatingCount > 0 && single.Track.TotalRating > 0)
            singleResponse.Rating = Math.Round(single.Track.TotalRating / single.Track.RatingCount, 2);

        return singleResponse;
    }

    public static PlayListResponse ToPlayListResponse(this PlayList playList) =>
        new()
        {
            Id = playList.Id,
            Title = playList.Title,
            Description = playList.Description
        };

    public static PlayListTrackResponse ToPlayListTrackResponse(this PlayListTrack playListTrack) =>
        new()
        {
            TrackId = playListTrack.TrackId,
            PlaylistId = playListTrack.PlaylistId
        };
}