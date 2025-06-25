using Deepo.Client.Web.Dto;

namespace Deepo.Client.Web.EventBus.Vinyl;

public class VinylFilterEventArgs : EventArgs
{
    public DateTime? SelectedDate { get; set; }
    public IEnumerable<GenreDto> SelectedGenres { get; set; } = [];
    public string Market { get; set; } = string.Empty;
}