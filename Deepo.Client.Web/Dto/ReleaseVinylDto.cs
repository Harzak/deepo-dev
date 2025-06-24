namespace Deepo.Client.Web.Dto;

[Serializable]
public class ReleaseVinylDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AuthorsNames { get; set; } = string.Empty;
    public IEnumerable<GenreDto> Genres { get; set; } = [];
    public DateTime ReleaseDate { get; set; }
    public string ThumbUrl { get; set; } = string.Empty;
    public string CoverUrl { get; set; } = string.Empty;
}