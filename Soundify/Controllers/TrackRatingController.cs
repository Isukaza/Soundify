using Helpers;
using Microsoft.AspNetCore.Mvc;
using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrackRatingController : Controller
{
    private readonly ITrackManager _trackManager;
    private readonly ITrackRatingManager _trackRatingManager;

    public TrackRatingController(ITrackManager trackManager, ITrackRatingManager trackRatingManager)
    {
        _trackManager = trackManager;
        _trackRatingManager = trackRatingManager;
    }

    [HttpGet("{trackId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrackRating(Guid trackId)
    {
        var trackRatings = await _trackRatingManager.GetTrackRatingByTrackIdAsync(trackId);
        return trackRatings.Count > 0
            ? await StatusCodes.Status200OK
                .ResultState("", trackRatings.Select(uf => uf.ToTrackRatingResponse()))
            : await StatusCodes.Status404NotFound.ResultState("No ratings");
    }

    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddTrackRating(TrackRatingCreateRequest ratingCreateRequest)
    {
        if (!await _trackManager.TrackExistsAsync(ratingCreateRequest.TrackId))
            return await StatusCodes.Status404NotFound.ResultState("Track not Found");

        if (await _trackRatingManager.TrackRatingExistsAsync(ratingCreateRequest.UserId, ratingCreateRequest.TrackId))
            return await StatusCodes.Status409Conflict.ResultState("Track rating already exists");

        var trackRating = await _trackRatingManager.AddTrackRatingAsync(ratingCreateRequest);
        return trackRating is not null
            ? await StatusCodes.Status200OK.ResultState("", trackRating.ToTrackRatingResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateTrackRating(TrackRatingUpdateRequest ratingUpdateRequest)
    {
        var trackRating = await _trackRatingManager.GetTrackRatingByIdAsync(ratingUpdateRequest.Id);
        if (trackRating is null)
            return await StatusCodes.Status404NotFound.ResultState("Rating not Found");

        return await _trackRatingManager.UpdateTrackRatingAsync(trackRating, ratingUpdateRequest)
            ? await StatusCodes.Status200OK.ResultState("", trackRating.ToTrackRatingResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTrackRating(Guid trackRatingId)
    {
        var trackRating = await _trackRatingManager.GetTrackRatingByIdAsync(trackRatingId);
        if (trackRating is null)
            return await StatusCodes.Status404NotFound.ResultState("Rating not Found");

        return await _trackRatingManager.DeleteTrackRatingAsync(trackRating)
            ? await StatusCodes.Status200OK.ResultState()
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}