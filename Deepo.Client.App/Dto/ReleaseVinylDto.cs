namespace Deepo.Client.App.Dto
{
    [Serializable]
    public class ReleaseVinylDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AuthorsNames { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string ThumbUrl { get; set; } = string.Empty;
    }
}
