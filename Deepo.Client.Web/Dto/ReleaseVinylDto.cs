using System.Collections.ObjectModel;

namespace Deepo.Client.Web.Dto;

[Serializable]
public class ReleaseVinylDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Collection<string> AuthorsNames { get;  } = [];
    public Collection<GenreDto> Genres { get; } = [];
    public DateTime ReleaseDate { get; set; }
    public string Country { get; set; } = string.Empty;
    public string Market { get; set; } = string.Empty;
    public string CoverUrl { get; set; } = string.Empty;
}