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
[Route("api/artist-social-media")]
[Authorize]
public class ArtistSocialMediaController : Controller
{
    private readonly IArtistSmManager _artistSmManager;
    private readonly IArtistManager _artistManager;

    public ArtistSocialMediaController(IArtistManager artistManager, IArtistSmManager artistSmManager)
    {
        _artistManager = artistManager;
        _artistSmManager = artistSmManager;
    }

    [HttpGet("{artistId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArtistSm(Guid artistId)
    {
        var socialMediaArtist = await _artistSmManager.GetSocialMediaByArtistIdAsync(artistId);
        return socialMediaArtist.Count == 0
            ? await StatusCodes.Status404NotFound.ResultState("The artist has no media links")
            : await StatusCodes.Status200OK.ResultState("", socialMediaArtist.Select(sma => sma.ToArtistSmResponse()));
    }

    [HttpPost("add")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddArtistSm(ArtistSmCreateRequest artistSmCreateRequest)
    {
        if (!await _artistManager.ArtistExistsAsync(artistSmCreateRequest.ArtistId))
            return await StatusCodes.Status404NotFound.ResultState("Artist doesn't exist");

        var userRole = HttpContext.User.Claims.GetUserRole();
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            if (!await _artistManager.IsPublisherOfArtistAsync(publisherId.Value, artistSmCreateRequest.ArtistId))
                return await StatusCodes.Status403Forbidden
                    .ResultState("You are not a publisher for this artist");
        }

        var socialMediaArtist = await _artistSmManager.AddSocialMediaAsync(artistSmCreateRequest);
        return socialMediaArtist is not null
            ? await StatusCodes.Status201Created
                .ResultState($"Artist social media with Id:{socialMediaArtist.Id} successfully added",
                    socialMediaArtist.ToArtistSmResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateArtistSm(ArtistSmUpdateRequest artistSmUpdateRequestRequest)
    {
        var userRole = HttpContext.User.Claims.GetUserRole();
        ArtistSocialMedia socialMedia;
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            socialMedia = await _artistSmManager
                .GetPublisherSocialMediaByIdAsync(publisherId.Value, artistSmUpdateRequestRequest.Id);
            if (socialMedia is null)
                return await StatusCodes.Status403Forbidden
                    .ResultState("You are not the publisher of this on social media");
        }
        else
        {
            socialMedia = await _artistSmManager.GetSocialMediaByIdAsync(artistSmUpdateRequestRequest.Id);
            if (socialMedia is null)
                return await StatusCodes.Status404NotFound.ResultState("Social media doesn't exist");
        }

        return await _artistSmManager.UpdateSocialMediaAsync(socialMedia, artistSmUpdateRequestRequest)
            ? await StatusCodes.Status200OK
                .ResultState($"Artist social media with Id:{socialMedia.Id} successfully updated",
                    socialMedia.ToArtistSmResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteArtistSm(Guid socialMediaId)
    {
        var userRole = HttpContext.User.Claims.GetUserRole();
        ArtistSocialMedia socialMedia;
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            socialMedia = await _artistSmManager
                .GetPublisherSocialMediaByIdAsync(publisherId.Value, socialMediaId);
            if (socialMedia is null)
                return await StatusCodes.Status403Forbidden
                    .ResultState("You are not the publisher of this on social media");
        }
        else
        {
            socialMedia = await _artistSmManager.GetSocialMediaByIdAsync(socialMediaId);
            if (socialMedia is null)
                return await StatusCodes.Status404NotFound.ResultState("Social media doesn't exist");
        }

        return await _artistSmManager.DeleteSocialMediaAsync(socialMedia)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}