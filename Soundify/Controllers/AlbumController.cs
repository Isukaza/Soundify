using Microsoft.AspNetCore.Mvc;

using Helpers;

using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumController : Controller
{
    private readonly IAlbumManager _albumManager;
    private readonly IArtistManager _artistManager;

    public AlbumController(IAlbumManager albumManager, IArtistManager artistManager)
    {
        _albumManager = albumManager;
        _artistManager = artistManager;
    }

    [HttpGet("{albumId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAlbum(Guid albumId)
    {
        var album = await _albumManager.GetAlbumByIdAsync(albumId);
        return album != null
            ? await StatusCodes.Status200OK.ResultState("", album.ToAlbumResponse())
            : await StatusCodes.Status404NotFound.ResultState("Not Found");
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAlbum(AlbumCreateRequest albumCreateRequest)
    {
        var artist = await _artistManager.GetArtistByIdAsync(albumCreateRequest.ArtistId);
        if (artist is null)
            return await StatusCodes.Status404NotFound.ResultState("Invalid artist");

        var album = await _albumManager.CreateAlbumAsync(artist, albumCreateRequest);
        return album != null
            ? await StatusCodes.Status201Created.ResultState("", album.ToAlbumResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAlbum(AlbumUpdateRequest albumUpdateRequest)
    {
        var album = await _albumManager.GetAlbumByIdAsync(albumUpdateRequest.Id);
        if (album is null)
            return await StatusCodes.Status404NotFound.ResultState("Not Found");

        return await _albumManager.UpdateAlbumAsync(album, albumUpdateRequest)
            ? await StatusCodes.Status200OK.ResultState("", album.ToAlbumResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAlbum(Guid albumId)
    {
        var album = await _albumManager.GetAlbumByIdAsync(albumId);
        if (album is null)
            return await StatusCodes.Status404NotFound.ResultState("Not Found");

        return await _albumManager.DeleteAlbumAsync(album)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}