using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Workers.Scheduling;

/// <summary>
/// Provides data for events that occur when a scheduled fetcher task is due for execution or has been aborted.
/// </summary>
public class DueEventArgs : EventArgs
{
    /// <summary>
    /// Gets the unique identifier of the fetcher that is due for execution.
    /// </summary>
    public string FetcherIdentifier { get; }

    /// <summary>
    /// Gets the unique identifier for this specific due event instance.
    /// </summary>
    public Guid EventIdentifier { get; }

    /// <summary>
    /// Gets the UTC date and time when the fetcher was scheduled to execute.
    /// </summary>
    public DateTime DueAt { get; }

    public DueEventArgs(string fetcherIdentifier, Guid eventIdentifier, DateTime dueAt)
    {
        FetcherIdentifier = fetcherIdentifier;
        EventIdentifier = eventIdentifier;
        DueAt = dueAt;
    }
}