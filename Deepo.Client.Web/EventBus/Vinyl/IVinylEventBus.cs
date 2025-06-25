namespace Deepo.Client.Web.EventBus.Vinyl;

public interface IVinylEventBus
{
    void FilterChanged(VinylFilterEventArgs args);
    void Subscribe(IVinylEventBusSubscriber subscriber);
    void Unsubscribe(IVinylEventBusSubscriber subscriber);
}