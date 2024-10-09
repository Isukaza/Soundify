using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
public class PlayListController : Controller
{
    private readonly IPlayListManager _playListManager;
    private readonly ITrackManager _trackManager;
    private readonly IAuthorizationManager _authorizationManager;

    public PlayListController(
        IPlayListManager playListManager,
        ITrackManager trackManager,
        IAuthorizationManager authorizationManager)
    {
        _playListManager = playListManager;
        _trackManager = trackManager;

        _authorizationManager = authorizationManager;
    }

    [HttpGet("{playListId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPlayList(Guid playListId)
    {
        var playList = await _playListManager.GetPlayListByIdAsync(playListId);
        if (playList is null)
            return await StatusCodes.Status404NotFound.ResultState("PlayList doesn't exist");

        var error = _authorizationManager.ValidateUserIdentity(
            HttpContext.User.Claims.ToList(),
            playList.UserId,
            UserRole.Admin,
            (userRole, compareRole) => userRole > compareRole);
        if (!string.IsNullOrEmpty(error))
            return await StatusCodes.Status403Forbidden.ResultState(error);

        return await StatusCodes.Status200OK.ResultState("", playList.ToPlayListResponse());
    }

    [HttpGet("tracks/{playListId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTracksPlayList(Guid playListId)
    {
        var userRole = HttpContext.User.Claims.GetUserRole();
        List<PlayListTrack> playListTracks;
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            playListTracks = await _playListManager.GetUserPlaylistTracksAsync(publisherId.Value, playListId);
            if (playListTracks.Count == 0)
                return await StatusCodes.Status403Forbidden
                    .ResultState("You are not a publisher for this playlist");
        }
        else
        {
            playListTracks = await _playListManager.GetTracksByPlayListIdAsync(playListId);
            if (playListTracks.Count == 0)
                return await StatusCodes.Status404NotFound.ResultState("PlayList doesn't exist");
        }

        return await StatusCodes.Status200OK
            .ResultState("", playListTracks.Select(plt => plt.ToPlayListTrackResponse()));
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreatePlayList(PlayListCreateRequest playListCreateRequest)
    {
        var userId = HttpContext.User.Claims.GetUserId();
        if (!userId.HasValue)
            return await StatusCodes.Status401Unauthorized
                .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

        var playList = await _playListManager.CreatePlayListAsync(userId.Value, playListCreateRequest);
        return playList is not null
            ? await StatusCodes.Status200OK
                .ResultState($"PlayList with Id:{playList.Id} successfully created", playList.ToPlayListResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("add-track")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddTrackToPlayList(PlayListAddTrackRequest playListAddTrackRequest)
    {
        var playListOwnerId = await _playListManager.GetPlaylistOwnerId(playListAddTrackRequest.PlayListId);
        if (!playListOwnerId.HasValue)
            return await StatusCodes.Status404NotFound.ResultState("Playlist doesn't exist");

        var error = _authorizationManager.ValidateUserIdentity(
            HttpContext.User.Claims.ToList(),
            playListOwnerId.Value,
            UserRole.Admin,
            (userRole, compareRole) => userRole > compareRole);
        if (!string.IsNullOrEmpty(error))
            return await StatusCodes.Status403Forbidden.ResultState(error);

        if (!await _trackManager.TrackExistsAsync(playListAddTrackRequest.TrackId))
            return await StatusCodes.Status404NotFound.ResultState("Track doesn't exist");

        var isTrackExist = await _playListManager
            .TrackExistsAsync(playListAddTrackRequest.PlayListId, playListAddTrackRequest.TrackId);
        if (isTrackExist)
            return await StatusCodes.Status409Conflict.ResultState("Track already exists");

        var playListTrack = await _playListManager.AddTrackToPlaylistAsync(playListAddTrackRequest);
        return playListTrack is not null
            ? await StatusCodes.Status200OK
                .ResultState($"Track with Id:{playListTrack.Id} successfully added to PlayList",
                    playListTrack.ToPlayListTrackResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePlayList(PlayListUpdateRequest playListUpdateRequest)
    {
        var playList = await _playListManager.GetPlayListByIdAsync(playListUpdateRequest.Id);
        if (playList is null)
            return await StatusCodes.Status404NotFound.ResultState("Playlist doesn't exist");

        var error = _authorizationManager.ValidateUserIdentity(
            HttpContext.User.Claims.ToList(),
            playList.UserId,
            UserRole.Admin,
            (userRole, compareRole) => userRole > compareRole);
        if (!string.IsNullOrEmpty(error))
            return await StatusCodes.Status403Forbidden.ResultState(error);

        return await _playListManager.UpdatePlayListAsync(playList, playListUpdateRequest)
            ? await StatusCodes.Status200OK
                .ResultState($"PlayList with Id:{playList.Id} successfully updated", playList.ToPlayListResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeletePlayList(Guid playListId)
    {
        var playList = await _playListManager.GetPlayListByIdAsync(playListId);
        if (playList is null)
            return await StatusCodes.Status404NotFound.ResultState("Playlist doesn't exist");

        var error = _authorizationManager.ValidateUserIdentity(
            HttpContext.User.Claims.ToList(),
            playList.UserId,
            UserRole.Admin,
            (userRole, compareRole) => userRole > compareRole);
        if (!string.IsNullOrEmpty(error))
            return await StatusCodes.Status403Forbidden.ResultState(error);

        return await _playListManager.DeletePlayListAsync(playList)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("remove-track")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveTrackFromPlayList(Guid playListId, Guid trackId)
    {
        var playListOwnerId = await _playListManager.GetPlaylistOwnerId(playListId);
        if (!playListOwnerId.HasValue)
            return await StatusCodes.Status404NotFound.ResultState("Playlist doesn't exist");

        var error = _authorizationManager.ValidateUserIdentity(
            HttpContext.User.Claims.ToList(),
            playListOwnerId.Value,
            UserRole.Admin,
            (userRole, compareRole) => userRole > compareRole);
        if (!string.IsNullOrEmpty(error))
            return await StatusCodes.Status403Forbidden.ResultState(error);

        var playListTrack = await _playListManager.GetTrackByTrackIdAsync(playListId, trackId);
        if (playListTrack is null)
            return await StatusCodes.Status404NotFound.ResultState("Track doesn't exist");

        return await _playListManager.RemoveTrackFromPlayList(playListTrack)
            ? await StatusCodes.Status200OK.ResultState("Remove successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}