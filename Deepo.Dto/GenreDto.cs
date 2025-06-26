namespace Deepo.Dto;

[Serializable]
public class GenreDto
{
    public Guid Identifier { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
}