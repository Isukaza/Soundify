using System.ComponentModel.DataAnnotations;
using Soundify.DAL.PostgreSQL.Models.db.Base;

namespace Soundify.DAL.PostgreSQL.Models.db;

public record TrackRating : BaseDbEntity
{
    [Range(1, 5)]
    public double Rating { get; set; }

    #region Relational
    
    public Guid UserId { get; set; }

    public Guid TrackId { get; set; }
    public Track Track { get; set; }
    
    #endregion
}