using Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumController : Controller
{
    [HttpGet("{albumId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArtist(Guid albumId)
    {
        var album = new
        {
            Id = albumId,
            Name = "New Album",
            Publisher = "New Publisher",
            Tracks = 50
        };

        return await StatusCodes.Status200OK.ResultState("", album);
    }
}