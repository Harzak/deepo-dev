using System;

namespace Deepo.Fetcher.Viewer.Models;

public sealed class FetcherExecutionEventArgs : EventArgs
{
    public string FetcherIdentifier { get; set; }
    public DateTime StartedAt { get; set; }

    public FetcherExecutionEventArgs(string fetcherIdentifier, DateTime startedAt)
    {
        FetcherIdentifier = fetcherIdentifier;
        StartedAt = startedAt;
    }
}
