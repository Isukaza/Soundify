using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Helpers;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Roles;
using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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