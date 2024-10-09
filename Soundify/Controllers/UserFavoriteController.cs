using Microsoft.AspNetCore.Mvc;

using Helpers;

using Soundify.DAL.PostgreSQL.Roles;
using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;

namespace Soundify.Controllers;

public class UserFavoriteController : Controller
{
    private readonly ITrackManager _trackManager;
    private readonly IUserFavoriteManager _userFavoriteManager;
    private readonly IAuthorizationManager _authorizationManager;

    public UserFavoriteController(
        ITrackManager trackManager,
        IUserFavoriteManager userFavoriteManager,
        IAuthorizationManager authorizationManager)
    {
        _trackManager = trackManager;
        _userFavoriteManager = userFavoriteManager;

        _authorizationManager = authorizationManager;
    }

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserFavorite(Guid userId)
    {
        var userIdJwt = HttpContext.User.Claims.GetUserId();
        if (!userIdJwt.HasValue)
            return await StatusCodes.Status401Unauthorized
                .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

        if (userId != userIdJwt.Value)
            return await StatusCodes.Status403Forbidden.ResultState("Cannot access another user's data");

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
        var userId = HttpContext.User.Claims.GetUserId();
        if (!userId.HasValue)
            return await StatusCodes.Status401Unauthorized
                .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

        if (!await _trackManager.TrackExistsAsync(favCreateRequest.TrackId))
            return await StatusCodes.Status404NotFound.ResultState("The track couldn't be found");

        if (await _userFavoriteManager.FavoriteExistsAsync(userId.Value, favCreateRequest.TrackId))
            return await StatusCodes.Status400BadRequest.ResultState("UserFavorite already exists");

        var userFavorite = await _userFavoriteManager.AddFavoriteAsync(userId.Value, favCreateRequest.TrackId);
        return userFavorite is not null
            ? await StatusCodes.Status201Created
                .ResultState($"UserFavorite with Id:{userFavorite.Id} successfully added",
                    userFavorite.ToUserFavoriteResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUserFavorite(Guid favoriteId)
    {
        var userFavorite = await _userFavoriteManager.GetFavoriteByIdAsync(favoriteId);
        if (userFavorite is null)
            return await StatusCodes.Status404NotFound.ResultState("User favorite doesn't exist");

        var error = _authorizationManager.ValidateUserIdentity(
            HttpContext.User.Claims.ToList(),
            userFavorite.UserId,
            UserRole.Admin,
            (userRole, compareRole) => userRole > compareRole);
        if (!string.IsNullOrEmpty(error))
            return await StatusCodes.Status403Forbidden.ResultState(error);

        return await _userFavoriteManager.DeleteFavoriteAsync(userFavorite)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}