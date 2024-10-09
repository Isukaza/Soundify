using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Helpers;

using Soundify.DAL.PostgreSQL.Roles;
using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrackRatingController : Controller
{
    private readonly ITrackManager _trackManager;
    private readonly ITrackRatingManager _trackRatingManager;
    private readonly IAuthorizationManager _authorizationManager;


    public TrackRatingController(
        ITrackManager trackManager,
        ITrackRatingManager trackRatingManager,
        IAuthorizationManager authorizationManager)
    {
        _trackManager = trackManager;
        _trackRatingManager = trackRatingManager;

        _authorizationManager = authorizationManager;
    }

    [HttpGet("{trackId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrackRating(Guid trackId)
    {
        var trackRatings = await _trackRatingManager.GetTrackRatingByTrackIdAsync(trackId);
        return trackRatings.Count > 0
            ? await StatusCodes.Status200OK
                .ResultState("", trackRatings.Select(uf => uf.ToTrackRatingResponse()))
            : await StatusCodes.Status404NotFound.ResultState("Track has no ratings");
    }

    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddTrackRating(TrackRatingCreateRequest ratingCreateRequest)
    {
        var userId = HttpContext.User.Claims.GetUserId();
        if (!userId.HasValue)
            return await StatusCodes.Status401Unauthorized
                .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

        if (!await _trackManager.TrackExistsAsync(ratingCreateRequest.TrackId))
            return await StatusCodes.Status404NotFound.ResultState("Track doesn't exist");

        if (await _trackRatingManager.TrackRatingExistsAsync(userId.Value, ratingCreateRequest.TrackId))
            return await StatusCodes.Status409Conflict.ResultState("Track rating already exists");

        var trackRating = await _trackRatingManager.AddTrackRatingAsync(userId.Value, ratingCreateRequest);
        return trackRating is not null
            ? await StatusCodes.Status200OK
                .ResultState($"Track rating with Id:{trackRating.Id} successfully added",
                    trackRating.ToTrackRatingResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateTrackRating(TrackRatingUpdateRequest ratingUpdateRequest)
    {
        var trackRating = await _trackRatingManager.GetTrackRatingByIdAsync(ratingUpdateRequest.Id);
        if (trackRating is null)
            return await StatusCodes.Status404NotFound.ResultState("Rating doesn't exist");

        var error = _authorizationManager.ValidateUserIdentity(
            HttpContext.User.Claims.ToList(),
            trackRating.UserId,
            UserRole.Admin,
            (userRole, compareRole) => userRole > compareRole);
        if (!string.IsNullOrEmpty(error))
            return await StatusCodes.Status403Forbidden.ResultState(error);

        return await _trackRatingManager.UpdateTrackRatingAsync(trackRating, ratingUpdateRequest)
            ? await StatusCodes.Status200OK
                .ResultState($"Track rating with Id:{trackRating.Id} successfully updated",
                    trackRating.ToTrackRatingResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTrackRating(Guid trackRatingId)
    {
        var trackRating = await _trackRatingManager.GetTrackRatingByIdAsync(trackRatingId);
        if (trackRating is null)
            return await StatusCodes.Status404NotFound.ResultState("Rating doesn't exist");

        var error = _authorizationManager.ValidateUserIdentity(
            HttpContext.User.Claims.ToList(),
            trackRating.UserId,
            UserRole.Admin,
            (userRole, compareRole) => userRole > compareRole);
        if (!string.IsNullOrEmpty(error))
            return await StatusCodes.Status403Forbidden.ResultState(error);

        return await _trackRatingManager.DeleteTrackRatingAsync(trackRating)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}