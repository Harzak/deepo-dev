using Deepo.Fetcher.Viewer.Models;
using System;

namespace Deepo.Fetcher.Viewer.Interfaces;

public interface IFetcherListener : IDisposable
{
    event EventHandler<HttpRequestLogEventArgs>? HttpRequestLogRowAdded;
    event EventHandler<GridModelEventArgs>? VinylReleaseRowAdded;
    event EventHandler<FetcherExecutionEventArgs>? FetcherExecutionRowAdded;

    void StartListener();
}
