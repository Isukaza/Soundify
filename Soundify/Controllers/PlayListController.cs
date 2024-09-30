using Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayListController : Controller
{
    [HttpGet("{playListId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArtist(Guid playListId)
    {
        var playList = new
        {
            Id = playListId,
            UserId = Guid.NewGuid(),
            Name = "New PlayList",
            Tracks = 10
        };

        return await StatusCodes.Status200OK.ResultState("", playList);
    }
}