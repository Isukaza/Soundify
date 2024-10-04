using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Soundify.DAL.PostgreSQL.Models.db;

namespace Soundify.DAL.PostgreSQL;

public class SoundifyDbContext : DbContext
{
    #region C-tor and Filelds

    private readonly ILogger<SoundifyDbContext> _logger;

    public SoundifyDbContext(DbContextOptions<SoundifyDbContext> options, ILogger<SoundifyDbContext> logger)
        : base(options)
    {
        _logger = logger;
    }

    #endregion

    #region DbSet

    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<ArtistSocialMedia> ArtistSocialMedias { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<PlayList> Playlists { get; set; }
    public DbSet<PlayListTrack> PlaylistTracks { get; set; }
    public DbSet<SingleTrack> SingleTracks { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<TrackRating> TrackRatings { get; set; }
    public DbSet<UserFavorite> UserFavorites { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Artists + ArtistSocialMedia

        modelBuilder.Entity<ArtistSocialMedia>()
            .HasOne(rt => rt.Artist)
            .WithMany(u => u.SocialMediaLinks)
            .HasForeignKey(rt => rt.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Artists + SingleTracks

        modelBuilder.Entity<SingleTrack>()
            .HasOne(rt => rt.Artist)
            .WithMany(u => u.Singles)
            .HasForeignKey(rt => rt.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Artists + Albums

        modelBuilder.Entity<Album>()
            .HasOne(rt => rt.Artist)
            .WithMany(u => u.Albums)
            .HasForeignKey(rt => rt.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Tracks + SingleTracks

        modelBuilder.Entity<Track>()
            .HasOne(rt => rt.Single)
            .WithOne(u => u.Track)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Tracks + Album

        modelBuilder.Entity<Track>()
            .HasOne(rt => rt.Album)
            .WithMany(u => u.Tracks)
            .HasForeignKey(rt => rt.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Tracks + Genre

        modelBuilder.Entity<Track>()
            .HasOne(rt => rt.Genre)
            .WithMany(u => u.Tracks)
            .HasForeignKey(rt => rt.GenreId)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion

        #region UserFavorites + Tracks

        modelBuilder.Entity<UserFavorite>()
            .HasOne(rt => rt.Track)
            .WithMany(u => u.UserFavorites)
            .HasForeignKey(rt => rt.TrackId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region TrackRatings + Tracks

        modelBuilder.Entity<TrackRating>()
            .HasOne(rt => rt.Track)
            .WithMany(u => u.Ratings)
            .HasForeignKey(rt => rt.TrackId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region PlayLists + PlaylistTracks

        modelBuilder.Entity<PlayListTrack>()
            .HasOne(rt => rt.PlayList)
            .WithMany(u => u.PlaylistTracks)
            .HasForeignKey(rt => rt.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region PlaylistTracks + Tracks

        modelBuilder.Entity<PlayListTrack>()
            .HasOne(rt => rt.Track)
            .WithMany(u => u.PlaylistTracks)
            .HasForeignKey(rt => rt.TrackId)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion
    }

    #region SaveFnction

    public bool SaveAndCompareAffectedRows(bool log = true)
    {
        try
        {
            var affectedRows = GetAffectedRows();
            return SaveChanges() >= affectedRows.Count;
        }
        catch (DbUpdateException ex)
        {
            return log && LogException(ex);
        }
    }

    public async Task<bool> SaveAndCompareAffectedRowsAsync(bool log = true)
    {
        try
        {
            var affectedRows = GetAffectedRows();
            return await SaveChangesAsync() >= affectedRows.Count;
        }
        catch (DbUpdateException ex)
        {
            return log && LogException(ex);
        }
    }

    private List<EntityEntry> GetAffectedRows()
    {
        var affectedRows = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added
                        || e.State == EntityState.Modified
                        || e.State == EntityState.Deleted)
            .ToList();

        foreach (var affectedRow in affectedRows)
        {
            var property = affectedRow.Properties.FirstOrDefault(p => p.Metadata.Name == "Modified");
            if (property != null)
                property.CurrentValue = DateTime.UtcNow;
        }

        return affectedRows;
    }

    private bool LogException(DbUpdateException ex)
    {
        _logger.LogInformation("[DB] Exception during saving {ex}", ex);
        _logger.LogInformation("Entries: ");
        foreach (var entry in ex.Entries)
            _logger.LogInformation("\t{Type} {EntryState}", entry.Entity.GetType(), entry.State);

        var st = new StackTrace(true);
        var frameDescriptions = st
            .GetFrames()
            .Where(f => !string.IsNullOrEmpty(f.GetFileName()))
            .Select(f => $"at {f.GetMethod()} in {f.GetFileName()}:line {f.GetFileLineNumber()}")
            .ToList();
        _logger
            .LogInformation("[DB] Exception stacktrace: {Frames}", string.Join(Environment.NewLine, frameDescriptions));
        return false;
    }

    #endregion
}