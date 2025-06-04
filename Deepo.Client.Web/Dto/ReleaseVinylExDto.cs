namespace Deepo.Client.Web.Dto;

[Serializable]
public class ReleaseVinylExDto : ReleaseVinylDto
{
    public string Label { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public Dictionary<string, string> Genres { get; } = [];
}

