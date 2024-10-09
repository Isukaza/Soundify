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
public class SingleController : Controller
{
    private readonly IArtistManager _artistManager;
    private readonly ISingleManager _singleManager;
    private readonly IGenreManager _genreManager;

    public SingleController(IArtistManager artistManager, ISingleManager singleManager, IGenreManager genreManager)
    {
        _artistManager = artistManager;
        _singleManager = singleManager;
        _genreManager = genreManager;
    }

    [HttpGet("{singleId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSingle(Guid singleId)
    {
        var single = await _singleManager.GetSingleTrack(singleId);
        return single is not null
            ? await StatusCodes.Status200OK.ResultState("", single.ToSingleResponse())
            : await StatusCodes.Status404NotFound.ResultState("Single doesn't exist");
    }

    [HttpPost("create")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateSingle(SingleCreateRequest singleCreateRequest)
    {
        var userRole = HttpContext.User.Claims.GetUserRole();
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            if (!await _artistManager.IsPublisherOfArtistAsync(publisherId.Value, singleCreateRequest.ArtistId))
                return await StatusCodes.Status403Forbidden.ResultState("You are not a publisher for this artist");
        }
        else
        {
            if (!await _artistManager.ArtistExistsAsync(singleCreateRequest.ArtistId))
                return await StatusCodes.Status404NotFound.ResultState("Artist doesn't exist");
        }

        if (!await _genreManager.GenreExistsAsync(singleCreateRequest.Track.GenreId))
            return await StatusCodes.Status404NotFound.ResultState("Genre doesn't exist");

        var single = await _singleManager.CreateSingleTrack(singleCreateRequest);
        return single is not null
            ? await StatusCodes.Status200OK
                .ResultState($"Single with Id:{single.Id} successfully created", single.ToSingleResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSingle(SingleUpdateRequest singleUpdateRequest)
    {
        var userRole = HttpContext.User.Claims.GetUserRole();
        SingleTrack single;
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            single = await _singleManager.GetPublisherSingleByIdAsync(publisherId.Value, singleUpdateRequest.Id);
            if (single is null)
                return await StatusCodes.Status403Forbidden
                    .ResultState("You are not a publisher for this single");
        }
        else
        {
            single = await _singleManager.GetSingleTrack(singleUpdateRequest.Id);
            if (single is null)
                return await StatusCodes.Status404NotFound.ResultState("Single doesn't exist");
        }

        return await _singleManager.UpdateSingleTrack(single, singleUpdateRequest)
            ? await StatusCodes.Status200OK
                .ResultState($"Single with Id:{single.Id} successfully updated", single.ToSingleResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteSingle(Guid singleId)
    {
        var userRole = HttpContext.User.Claims.GetUserRole();
        SingleTrack single;
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            single = await _singleManager.GetPublisherSingleByIdAsync(publisherId.Value, singleId);
            if (single is null)
                return await StatusCodes.Status403Forbidden
                    .ResultState("You are not a publisher for this single");
        }
        else
        {
            single = await _singleManager.GetSingleTrack(singleId);
            if (single is null)
                return await StatusCodes.Status404NotFound.ResultState("Single doesn't exist");
        }

        return await _singleManager.DeleteSingleTrack(single)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}