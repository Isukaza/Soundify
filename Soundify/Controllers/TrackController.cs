using Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrackController : Controller
{
    [HttpGet("{trackId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrack(Guid trackId)
    {
        var track = new
        {
            Id = trackId,
            Name = "New Track",
            Artist = "New Artist",
            Duration = new TimeSpan(0, 0, 18)
        };
        
        return await StatusCodes.Status200OK.ResultState("", track);
    }
}