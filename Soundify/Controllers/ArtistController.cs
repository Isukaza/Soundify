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
public class ArtistController : Controller
{
    private readonly IArtistManager _artistManager;

    public ArtistController(IArtistManager artistManager)
    {
        _artistManager = artistManager;
    }

    [HttpGet("{artistId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArtist(Guid artistId)
    {
        var artist = await _artistManager.GetArtistByIdAsync(artistId);
        return artist != null
            ? await StatusCodes.Status200OK.ResultState("", artist.ToArtistResponse())
            : await StatusCodes.Status404NotFound.ResultState("Artist doesn't exist");
    }

    [HttpPost("create")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdmin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateArtist(ArtistCreateRequest artistCreateRequest)
    {
        var artist = await _artistManager.CreateArtistAsync(artistCreateRequest);
        return artist != null
            ? await StatusCodes.Status201Created
                .ResultState($"Artist with Id:{artist.Id} successfully created", artist.ToArtistResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateArtist(ArtistUpdateRequest artistUpdateRequest)
    {
        var userRole = HttpContext.User.Claims.GetUserRole();
        Artist artist;
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            artist = await _artistManager.GetPublisherArtistByIdAsync(publisherId.Value, artistUpdateRequest.Id);
            if (artist == null)
                return await StatusCodes.Status403Forbidden.ResultState("You are not a publisher for this artist");
        }
        else
        {
            artist = await _artistManager.GetArtistByIdAsync(artistUpdateRequest.Id);
            if (artist is null)
                return await StatusCodes.Status404NotFound.ResultState("Artist doesn't exist");
        }

        return await _artistManager.UpdateArtistAsync(artist, artistUpdateRequest)
            ? await StatusCodes.Status200OK
                .ResultState($"Artist with Id:{artist.Id} successfully updated", artist.ToArtistResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteArtist(Guid artistId)
    {
        var userRole = HttpContext.User.Claims.GetUserRole();
        Artist artist;
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            artist = await _artistManager.GetPublisherArtistByIdAsync(publisherId.Value, artistId);
            if (artist == null)
                return await StatusCodes.Status403Forbidden.ResultState("You are not a publisher for this artist");
        }
        else
        {
            artist = await _artistManager.GetArtistByIdAsync(artistId);
            if (artist is null)
                return await StatusCodes.Status404NotFound.ResultState("Artist doesn't exist");
        }

        return await _artistManager.DeleteArtistAsync(artist)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}