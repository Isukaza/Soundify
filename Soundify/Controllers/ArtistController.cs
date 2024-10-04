using Microsoft.AspNetCore.Mvc;

using Helpers;

using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
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
            : await StatusCodes.Status404NotFound.ResultState("Not Found");
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateArtist(ArtistCreateRequest artistCreateRequest)
    {
        var artist = await _artistManager.CreateArtistAsync(artistCreateRequest);
        return artist != null
            ? await StatusCodes.Status201Created.ResultState("", artist.ToArtistResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateArtist(ArtistUpdateRequest artistCreateRequest)
    {
        var artist = await _artistManager.GetArtistByIdAsync(artistCreateRequest.Id);
        if (artist is null)
            return await StatusCodes.Status404NotFound.ResultState("Not Found");

        return await _artistManager.UpdateArtistAsync(artist, artistCreateRequest)
            ? await StatusCodes.Status200OK.ResultState("", artist.ToArtistResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteArtist(Guid artistId)
    {
        var artist = await _artistManager.GetArtistByIdAsync(artistId);
        if (artist is null)
            return await StatusCodes.Status404NotFound.ResultState("Not Found");

        return await _artistManager.DeleteArtistAsync(artist)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}