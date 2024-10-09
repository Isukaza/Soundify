using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Helpers;

using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.DAL.PostgreSQL.Roles;
using Soundify.Managers.Interfaces;
using Soundify.Models;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
            : await StatusCodes.Status404NotFound.ResultState("Album doesn't exist");
    }

    [HttpPost("create")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAlbum(AlbumCreateRequest albumCreateRequest)
    {
        var userRole = HttpContext.User.Claims.GetUserRole();
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            if (!await _artistManager.IsPublisherOfArtistAsync(publisherId.Value, albumCreateRequest.ArtistId))
                return await StatusCodes.Status403Forbidden.ResultState("You are not an publisher for this artist");
        }
        else
        {
            if (!await _artistManager.ArtistExistsAsync(albumCreateRequest.ArtistId))
                return await StatusCodes.Status404NotFound.ResultState("Artist does not exist");
        }

        var album = await _albumManager.CreateAlbumAsync(albumCreateRequest);
        return album != null
            ? await StatusCodes.Status201Created
                .ResultState($"Album with Id:{album.Id} successfully created", album.ToAlbumResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpPost("update")]
    [Authorize(Policy = nameof(RolePolicy.RequireContentEditors))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAlbum(AlbumUpdateRequest albumUpdateRequest)
    {
        var userRole = HttpContext.User.Claims.GetUserRole();
        Album album;
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            album = await _albumManager.GetPublisherAlbumByIdAsync(publisherId.Value, albumUpdateRequest.Id);
            if (album is null)
                return await StatusCodes.Status403Forbidden
                    .ResultState("You are not a publisher for this album");
        }
        else
        {
            album = await _albumManager.GetAlbumByIdAsync(albumUpdateRequest.Id);
            if (album is null)
                return await StatusCodes.Status404NotFound.ResultState("Album doesn't exist");
        }

        return await _albumManager.UpdateAlbumAsync(album, albumUpdateRequest)
            ? await StatusCodes.Status200OK
                .ResultState($"Album with Id:{album.Id} successfully updated", album.ToAlbumResponse())
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    [HttpDelete("delete")]
    [Authorize(Policy = nameof(RolePolicy.RequireAnyAdminOrPublisher))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAlbum(Guid albumId)
    {
        var userRole = HttpContext.User.Claims.GetUserRole();
        Album album;
        if (userRole.HasValue && userRole.Value == UserRole.Publisher)
        {
            var publisherId = HttpContext.User.Claims.GetUserId();
            if (!publisherId.HasValue)
                return await StatusCodes.Status401Unauthorized
                    .ResultState("Authorization failed due to an invalid or missing userId in the provided token");

            album = await _albumManager.GetPublisherAlbumByIdAsync(publisherId.Value, albumId);
            if (album is null)
                return await StatusCodes.Status403Forbidden
                    .ResultState("You are not a publisher for this album");
        }
        else
        {
            album = await _albumManager.GetAlbumByIdAsync(albumId);
            if (album is null)
                return await StatusCodes.Status404NotFound.ResultState("Album does not exist");
        }

        return await _albumManager.DeleteAlbumAsync(album)
            ? await StatusCodes.Status200OK.ResultState("Delete successful")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}