using System.ComponentModel.DataAnnotations;
using Soundify.DAL.PostgreSQL.Models.db.Base;

namespace Soundify.DAL.PostgreSQL.Models.db;

public record Track : BaseDbEntity
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    public int Duration { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    [Required]
    [StringLength(255)]
    public string FilePath { get; set; }

    public double TotalRating { get; set; }

    public int RatingCount { get; set; }

    #region Relational

    public SingleTrack? Single { get; set; }

    public Guid? AlbumId { get; set; }
    public Album? Album { get; set; }

    public Guid GenreId { get; set; }
    public Genre Genre { get; set; }

    public ICollection<PlayListTrack> PlaylistTracks { get; set; } = new HashSet<PlayListTrack>();
    public ICollection<UserFavorite> UserFavorites { get; set; } = new HashSet<UserFavorite>();
    public ICollection<TrackRating> Ratings { get; set; } = new HashSet<TrackRating>();

    #endregion
}