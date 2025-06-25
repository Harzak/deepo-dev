namespace Deepo.Client.Web.EventBus.Vinyl;

public interface IVinylEventBusSubscriber
{
    void OnFilterChanged(VinylFilterEventArgs args);
}