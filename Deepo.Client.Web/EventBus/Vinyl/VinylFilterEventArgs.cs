using Deepo.Client.Web.Dto;

namespace Deepo.Client.Web.EventBus.Vinyl;

public class VinylFilterEventArgs : EventArgs
{
    public DateTime Date { get; set; }
    public IEnumerable<GenreDto> Genres { get; set; } = [];
    public IEnumerable<string> Markets { get; set; } = [];
}