using Microsoft.JSInterop;
using System.Collections.Concurrent;

namespace Deepo.Client.Web.EventBus.Vinyl;


public class VinylEventBus : IVinylEventBus
{
    private readonly object _lock;
    private readonly List<IVinylEventBusSubscriber> _subscribers;

    public VinylEventBus()
    {
        _lock = new object();
        _subscribers = [];
    }

    public void FilterChanged(VinylFilterEventArgs args)
    {
        IVinylEventBusSubscriber[] subscriberSnapshot;
        lock (_lock)
        {
            subscriberSnapshot = _subscribers.ToArray();
        }

        foreach (var subscriber in subscriberSnapshot)
        {
            subscriber.OnFilterChanged(args);   
        }
    }

    public void Subscribe(IVinylEventBusSubscriber subscriber)
    {
        lock (_lock)
        {
            if (!_subscribers.Contains(subscriber))
            {
                _subscribers.Add(subscriber);
            }
        }
    }

    public void Unsubscribe(IVinylEventBusSubscriber subscriber)
    {
        lock (_lock)
        {
            _subscribers.Remove(subscriber);
        }
    }
}