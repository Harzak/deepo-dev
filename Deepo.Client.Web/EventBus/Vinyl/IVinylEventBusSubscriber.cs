namespace Deepo.Client.Web.EventBus.Vinyl;

public interface IVinylEventBusSubscriber
{
    Task OnFilterChangedAsync(VinylFilterEventArgs args);
}