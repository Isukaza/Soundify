using Helpers;
using Microsoft.AspNetCore.Mvc;
using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Controllers;

[ApiController]
[Route("api/artist-social-media")]
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
        if (socialMediaArtist.Count == 0)
            return await StatusCodes.Status404NotFound.ResultState("The artist has no media links");
        
        return await StatusCodes.Status200OK
            .ResultState("", socialMediaArtist.Select(sma => sma.ToArtistSmResponse()));
    }

    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddArtistSm(ArtistSmCreateRequest artistSmCreateRequest)
    {
        if (!await _artistManager.ArtistExistsAsync(artistSmCreateRequest.ArtistId))
            return await StatusCodes.Status404NotFound.ResultState("Artist not found");

        var socialMediaArtist = await _artistSmManager.AddSocialMediaAsync(artistSmCreateRequest);
        return socialMediaArtist is not null
            ? await StatusCodes.Status201Created.ResultState("", socialMediaArtist.ToArtistSmResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateArtistSm(ArtistSmUpdateRequest artistSmUpdateRequestRequest)
    {
        var socialMedia = await _artistSmManager.GetSocialMediaByIdAsync(artistSmUpdateRequestRequest.Id);
        if(socialMedia is null)
            return await StatusCodes.Status404NotFound.ResultState("Social media not found");
        
        return await _artistSmManager.UpdateSocialMediaAsync(socialMedia, artistSmUpdateRequestRequest)
            ? await StatusCodes.Status200OK.ResultState("", socialMedia.ToArtistSmResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteArtistSm(Guid socialMediaId)
    {
        var socialMedia = await _artistSmManager.GetSocialMediaByIdAsync(socialMediaId);
        if (socialMedia is null)
            return await StatusCodes.Status404NotFound.ResultState("Social media not found");
        
        return await _artistSmManager.DeleteSocialMediaAsync(socialMedia)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}