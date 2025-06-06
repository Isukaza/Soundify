using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Helpers;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Roles;
using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Filtration;
using Soundify.Models.Request.Update;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrackController : Controller
{
    private readonly ITrackManager _trackManager;
    private readonly IGenreManager _genreManager;

    public TrackController(ITrackManager trackManager, IGenreManager genreManager)
    {
        _trackManager = trackManager;
        _genreManager = genreManager;
    }

    [HttpGet("{trackId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrack(Guid trackId)
    {
        var track = await _trackManager.GetTrackByIdAsync(trackId);
        return track is not null
            ? await StatusCodes.Status200OK.ResultState("", track.ToTrackResponse())
            : await StatusCodes.Status404NotFound.ResultState("Track doesn't exist");
    }

    [HttpPost("get-tracks-by-filter")]
    public async Task<IActionResult> GetTracksByFilter(TrackFilter filter)
    {
        var pagedTracksResult = await _trackManager.GetTracksByFilterAsync(filter);

        if (pagedTracksResult.HasNextPage)
            Response.Headers["X-Next-Page"] = $"{filter.Page + 1}";

        return pagedTracksResult.Tracks is not null && pagedTracksResult.Tracks.Count > 0
            ? await StatusCodes.Status200OK.ResultState("", pagedTracksResult.Tracks)
            : await StatusCodes.Status404NotFound.ResultState("Track doesn't exist");
    }

    [HttpPost("get-upload-token")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUploadToken([FromBody] Guid trackId)
    {
        var trackData = await _trackManager.GetTrackUploadTokenDataAsync(trackId);
        if (trackData == null)
            return await StatusCodes.Status404NotFound.ResultState("Track doesn't exist");

        var userId = HttpContext.User.Claims.GetUserId();
        var userRole = HttpContext.User.Claims.GetUserRole();
        if (!userId.HasValue || !userRole.HasValue)
            return await StatusCodes.Status401Unauthorized
                .ResultState("Authorization failed due to an invalid or missing userId or role in the provided token");

        if (userRole.Value == UserRole.Publisher && trackData.PublisherId != userId.Value)
            return await StatusCodes.Status403Forbidden
                .ResultState("You are not the publisher for this track");

        var token = await _trackManager.GenerateUploadTokenAsync(userId.Value, trackId, trackData);
        return await StatusCodes.Status200OK.ResultState("The upload token was successfully generated", token);
    }

    [HttpPost("create")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateTrack(TrackCreateRequest trackCreateRequest)
    {
        var genre = await _genreManager.GetGenreByIdAsync(trackCreateRequest.GenreId);
        if (genre is null)
            return await StatusCodes.Status404NotFound.ResultState("Genre doesn't exist");

        var track = await _trackManager.CreateTrackAsync(trackCreateRequest, genre);
        return track is not null
            ? await StatusCodes.Status201Created.ResultState("", track.ToTrackResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateTrack(TrackUpdateRequest trackUpdateRequest)
    {
        var userRole = HttpContext.User.Claims.GetUserRole();
        Track track;
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            track = await _trackManager.IsTrackInAlbumOrSingleAsync(trackUpdateRequest.Id)
                ? await _trackManager.GetPublisherTrackByIdAsync(publisherId.Value, trackUpdateRequest.Id)
                : await _trackManager.GetTrackByIdAsync(trackUpdateRequest.Id);
            if (track is null)
                return await StatusCodes.Status403Forbidden
                    .ResultState("You are not a publisher for this track");
        }
        else
        {
            track = await _trackManager.GetTrackByIdAsync(trackUpdateRequest.Id);
            if (track is null)
                return await StatusCodes.Status404NotFound.ResultState("Track doesn't exist");
        }

        return await _trackManager.UpdateTrackAsync(track, trackUpdateRequest)
            ? await StatusCodes.Status200OK.ResultState("", track.ToTrackResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTrack(Guid trackId)
    {
        var userRole = HttpContext.User.Claims.GetUserRole();
        Track track;
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            track = await _trackManager.IsTrackInAlbumOrSingleAsync(trackId)
                ? await _trackManager.GetPublisherTrackByIdAsync(publisherId.Value, trackId)
                : await _trackManager.GetTrackByIdAsync(trackId);
            if (track is null)
                return await StatusCodes.Status403Forbidden
                    .ResultState("You are not a publisher for this track");
        }
        else
        {
            track = await _trackManager.GetTrackByIdAsync(trackId);
            if (track is null)
                return await StatusCodes.Status404NotFound.ResultState("Track doesn't exist");
        }

        return await _trackManager.DeleteTrackAsync(track)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}