using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;

namespace Soundify.Models.Request.Filtration;

public class Filter : IFilter
{
    public Guid? TrackId { get; init; }

    [MinLength(3, ErrorMessage = "The Name must be at least 3 characters long.")]
    [MaxLength(100, ErrorMessage = "The Name must be at most 100 characters long.")]
    public string? TrackName { get; init; }

    public Guid? AlbumId { get; init; }

    [MinLength(3, ErrorMessage = "The Album Name must be at least 3 characters long.")]
    [MaxLength(100, ErrorMessage = "The Album Name must be at most 100 characters long.")]
    public string? AlbumName { get; init; }

    public Guid? ArtistId { get; init; }

    [MinLength(3, ErrorMessage = "The Artist Name must be at least 3 characters long.")]
    [MaxLength(100, ErrorMessage = "The Artist Name must be at most 100 characters long.")]
    public string? ArtistName { get; init; }

    [DefaultValue(1)]
    [Range(1, 1000, ErrorMessage = "Page must be between 1 and 1000.")]
    public required int Page { get; init; } = 1;

    [DefaultValue(20)]
    [Range(1, 100, ErrorMessage = "Size must be between 1 and 100.")]
    public required int Size { get; init; } = 20;
}