using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Helpers;

using Soundify.DAL.PostgreSQL.Roles;
using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
        return genre is null
            ? await StatusCodes.Status404NotFound.ResultState("Genre doesn't exist")
            : await StatusCodes.Status200OK.ResultState("", genre.ToGenreResponse());
    }

    [HttpPost("create")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdmin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateGenre(GenreCreateRequest genreCreateRequest)
    {
        var genre = await _genreManager.CreateGenreAsync(genreCreateRequest);
        return genre is not null
            ? await StatusCodes.Status201Created
                .ResultState($"Genre with Id:{genre.Id} successfully created", genre.ToGenreResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdmin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateGenre(GenreUpdateRequest genreUpdateRequest)
    {
        var genre = await _genreManager.GetGenreByIdAsync(genreUpdateRequest.Id);
        if (genre is null)
            return await StatusCodes.Status404NotFound.ResultState("Genre doesn't exist");

        return await _genreManager.UpdateGenreAsync(genre, genreUpdateRequest)
            ? await StatusCodes.Status200OK
                .ResultState($"Genre with Id:{genre.Id} successfully updated", genre.ToGenreResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdmin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteGenre(Guid genreId)
    {
        var genre = await _genreManager.GetGenreByIdAsync(genreId);
        if (genre is null)
            return await StatusCodes.Status404NotFound.ResultState("Genre doesn't exist");

        if (await _genreManager.HasRelatedRecordsAsync(genreId))
            return await StatusCodes.Status409Conflict
                .ResultState("You can't delete a genre that has dependent tracks");

        return await _genreManager.DeleteGenreAsync(genre)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}