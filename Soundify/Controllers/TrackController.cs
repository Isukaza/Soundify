using Microsoft.AspNetCore.Mvc;

using Helpers;

using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
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
            : await StatusCodes.Status404NotFound.ResultState("Not Found");
    }
    
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateTrack(TrackCreateRequest trackCreateRequest)
    {
        var genre = await _genreManager.GetGenreByIdAsync(trackCreateRequest.GenreId);
        if (genre is null)
            return await StatusCodes.Status404NotFound.ResultState("Genre not found");
        
        var track = await _trackManager.CreateTrackAsync(trackCreateRequest, genre);
        return track is not null
            ? await StatusCodes.Status201Created.ResultState("", track.ToTrackResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateTrack(TrackUpdateRequest trackUpdateRequest)
    {
        var track = await _trackManager.GetTrackByIdAsync(trackUpdateRequest.Id);
        if(track is null)
            return await StatusCodes.Status404NotFound.ResultState("Not Found");

        return await _trackManager.UpdateTrackAsync(track, trackUpdateRequest)
            ? await StatusCodes.Status200OK.ResultState("", track.ToTrackResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTrack(Guid trackId)
    {
        var track = await _trackManager.GetTrackByIdAsync(trackId);
        if (track is null)
            return await StatusCodes.Status404NotFound.ResultState("Not Found");

        return await _trackManager.DeleteTrackAsync(track)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}