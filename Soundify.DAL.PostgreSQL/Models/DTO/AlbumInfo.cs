namespace Soundify.DAL.PostgreSQL.Models.DTO;

public class AlbumInfo
{
    public Guid Id { get; set; }
    public Guid ArtistId { get; set; }
    public string ArtistName { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string CoverFilePath { get; set; }
}