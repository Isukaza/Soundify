using Microsoft.AspNetCore.Mvc;

using Helpers;

using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayListController : Controller
{
    private readonly IPlayListManager _playListManager;
    private readonly ITrackManager _trackManager;

    public PlayListController(IPlayListManager playListManager, ITrackManager trackManager)
    {
        _playListManager = playListManager;
        _trackManager = trackManager;
    }

    [HttpGet("{playListId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPlayList(Guid playListId)
    {
        var playList = await _playListManager.GetPlayListByIdAsync(playListId);
        return playList is not null
            ? await StatusCodes.Status200OK.ResultState("", playList.ToPlayListResponse())
            : await StatusCodes.Status404NotFound.ResultState();
    }

    [HttpGet("tracks/{playListId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTracksPlayList(Guid playListId)
    {
        var playListTrack = await _playListManager.GetTracksByPlayListIdAsync(playListId);
        return playListTrack.Count > 0
            ? await StatusCodes.Status200OK
                .ResultState("", playListTrack.Select(plt => plt.ToPlayListTrackResponse()))
            : await StatusCodes.Status404NotFound.ResultState();
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreatePlayList(PlayListCreateRequest playListCreateRequest)
    {
        var playList = await _playListManager.CreatePlayListAsync(playListCreateRequest);
        return playList is not null
            ? await StatusCodes.Status200OK.ResultState("", playList.ToPlayListResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("add-track")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddTrackToPlayList(PlayListAddTrackRequest playListAddTrackRequest)
    {
        if (!await _playListManager.PlayListExistExistsAsync(playListAddTrackRequest.PlayListId))
            return await StatusCodes.Status404NotFound.ResultState("Playlist doesn't exist");
        
        if(!await _trackManager.TrackExistsAsync(playListAddTrackRequest.TrackId))
            return await StatusCodes.Status404NotFound.ResultState("Track doesn't exist");

        var isTrackExist = await _playListManager
            .TrackExistExistsAsync(playListAddTrackRequest.PlayListId, playListAddTrackRequest.TrackId);
        if (isTrackExist)
            return await StatusCodes.Status409Conflict.ResultState("Track already exists");

        var playListTrack = await _playListManager.AddTrackToPlaylistAsync(playListAddTrackRequest);
        return playListTrack is not null
            ? await StatusCodes.Status200OK.ResultState("", playListTrack.ToPlayListTrackResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePlayList(PlayListUpdateRequest playListUpdateRequest)
    {
        var playList = await _playListManager.GetPlayListByIdAsync(playListUpdateRequest.Id);
        if (playList is null)
            return await StatusCodes.Status404NotFound.ResultState("Playlist doesn't exist");

        return await _playListManager.UpdatePlayListAsync(playList, playListUpdateRequest)
            ? await StatusCodes.Status200OK.ResultState("", playList.ToPlayListResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeletePlayList(Guid playListId)
    {
        var playList = await _playListManager.GetPlayListByIdAsync(playListId);
        if (playList is null)
            return await StatusCodes.Status404NotFound.ResultState("Playlist doesn't exist");
        
        return await _playListManager.DeletePlayListAsync(playList)
            ? await StatusCodes.Status200OK.ResultState()
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("remove-track")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveTrackFromPlayList(Guid playListId, Guid trackId)
    {
        var playListTrack = await _playListManager.GetTrackByTrackIdAsync(playListId, trackId);
        if (playListTrack is null)
            return await StatusCodes.Status404NotFound.ResultState("Track Not Found");

        return await _playListManager.RemoveTrackFromPlayList(playListTrack)
            ? await StatusCodes.Status200OK.ResultState()
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}