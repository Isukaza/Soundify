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
        ArgumentNullException.ThrowIfNull(track, nameof(track));
        
        var trackResponse = new TrackResponse
        {
            TrackId = track.Id,
            TrackName = track.Title,
            FilePath = track.FilePath,
            Duration = track.Duration,
            Genre = track.Genre?.Name ?? string.Empty
        };

        if (track.Album != null)
        {
            trackResponse.AlbumId = track.Album.Id;
            trackResponse.AlbumName = track.Album.Title;

            if (track.Album.Artist != null)
            {
                trackResponse.ArtistId = track.Album.Artist.Id;
                trackResponse.ArtistName = track.Album.Artist.Name;
            }
        }

        if (track.RatingCount > 0)
            trackResponse.TotalRating = Math.Round(track.TotalRating / track.RatingCount, 2);

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