using Microsoft.AspNetCore.Mvc;

using Helpers;

using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenreController : Controller
{
    private readonly IGenreManager _genreManager;

    public GenreController(IGenreManager genreManager)
    {
        _genreManager = genreManager;
    }

    [HttpGet("{genreId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGenre(Guid genreId)
    {
        var genre = await _genreManager.GetGenreByIdAsync(genreId);
        if (genre is null)
            return await StatusCodes.Status404NotFound.ResultState("Not Found");

        return await StatusCodes.Status200OK.ResultState("", genre.ToGenreResponse());
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateGenre(GenreCreateRequest genreCreateRequest)
    {
        var genre = await _genreManager.CreateGenreAsync(genreCreateRequest);
        return genre is not null
            ? await StatusCodes.Status201Created.ResultState("", genre.ToGenreResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateGenre(GenreUpdateRequest genreUpdateRequest)
    {
        var genre = await _genreManager.GetGenreByIdAsync(genreUpdateRequest.Id);
        if (genre is null)
            return await StatusCodes.Status404NotFound.ResultState("Not Found");

        return await _genreManager.UpdateGenreAsync(genre, genreUpdateRequest)
            ? await StatusCodes.Status200OK.ResultState("", genre.ToGenreResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteGenre(Guid genreId)
    {
        var genre = await _genreManager.GetGenreByIdAsync(genreId);
        if (genre is null)
            return await StatusCodes.Status404NotFound.ResultState("Not Found");
        
        if (await _genreManager.HasRelatedRecordsAsync(genreId))
            return await StatusCodes.Status400BadRequest
                .ResultState("You can't delete a genre that has dependent tracks");

        return await _genreManager.DeleteGenreAsync(genre)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}