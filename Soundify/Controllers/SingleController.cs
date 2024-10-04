using Microsoft.AspNetCore.Mvc;

using Helpers;

using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SingleController : Controller
{
    private readonly ISingleManager _singleManager;

    public SingleController(ISingleManager singleManager)
    {
        _singleManager = singleManager;
    }

    [HttpGet("{singleId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSingle(Guid singleId)
    {
        var single = await _singleManager.GetSingleTrack(singleId);
        return single is not null
            ? await StatusCodes.Status200OK.ResultState("", single.ToSingleResponse())
            : await StatusCodes.Status404NotFound.ResultState("Not found");
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateSingle(SingleCreateRequest singleCreateRequest)
    {
        var single = await _singleManager.CreateSingleTrack(singleCreateRequest);
        return single is not null
            ? await StatusCodes.Status200OK.ResultState("", single.ToSingleResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSingle(SingleUpdateRequest singleUpdateRequest)
    {
        var single = await _singleManager.GetSingleTrack(singleUpdateRequest.Id);
        if (single is null)
            return await StatusCodes.Status404NotFound.ResultState("Not found");

        return await _singleManager.UpdateSingleTrack(single, singleUpdateRequest)
            ? await StatusCodes.Status200OK.ResultState("", single.ToSingleResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteSingle(Guid singleId)
    {
        var single = await _singleManager.GetSingleTrack(singleId);
        if (single is null)
            return await StatusCodes.Status404NotFound.ResultState("Not found");

        return await _singleManager.DeleteSingleTrack(single)
            ? await StatusCodes.Status200OK.ResultState()
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}