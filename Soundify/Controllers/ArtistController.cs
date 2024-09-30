using Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistController : Controller
{
    [HttpGet("{artistId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArtist(Guid artistId)
    {
        var artist = new
        {
            Id = artistId,
            Name = "New Artist",
            Tracks = 100
        };

        return await StatusCodes.Status200OK.ResultState("", artist);
    }
}