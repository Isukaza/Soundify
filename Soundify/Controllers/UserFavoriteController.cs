using Microsoft.AspNetCore.Mvc;

using Helpers;

using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;

namespace Soundify.Controllers;

public class UserFavoriteController : Controller
{
    private readonly ITrackManager _trackManager;
    private readonly IUserFavoriteManager _userFavoriteManager;

    public UserFavoriteController(ITrackManager trackManager, IUserFavoriteManager userFavoriteManager)
    {
        _trackManager = trackManager;
        _userFavoriteManager = userFavoriteManager;
    }

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserFavorite(Guid userId)
    {
        var userFavorites = await _userFavoriteManager.GetFavoriteByUserIdAsync(userId);
        return userFavorites.Count > 0
            ? await StatusCodes.Status200OK
                .ResultState("", userFavorites.Select(uf => uf.ToUserFavoriteResponse()))
            : await StatusCodes.Status404NotFound.ResultState("The user has no track favorite");
    }

    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddUserFavorite(UserFavoriteCreateRequest favCreateRequest)
    {
        if (!await _trackManager.TrackExistsAsync(favCreateRequest.TrackId))
            return await StatusCodes.Status404NotFound.ResultState("The track could not be found");
        
        if (await _userFavoriteManager.FavoriteExistsAsync(favCreateRequest.UserId, favCreateRequest.TrackId))
            return await StatusCodes.Status400BadRequest.ResultState("UserFavorite already exists");

        var userFavorite = await _userFavoriteManager.AddFavoriteAsync(favCreateRequest);
        return userFavorite is not null
            ? await StatusCodes.Status201Created.ResultState("", userFavorite.ToUserFavoriteResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUserFavorite(Guid favoriteId)
    {
        var userFavorite = await _userFavoriteManager.GetFavoriteByIdAsync(favoriteId);
        if (userFavorite is null)
            return await StatusCodes.Status404NotFound.ResultState("User favorite not found");

        return await _userFavoriteManager.DeleteFavoriteAsync(userFavorite)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}