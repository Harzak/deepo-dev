using Deepo.Fetcher.Library.Workers.Scheduling;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Interfaces;

/// <summary>
/// Defines the contract for a scheduled due event that manages the timing of fetcher task execution.
/// This interface extends <see cref="IHostedService"/> to provide lifecycle management for long-running
/// scheduled tasks within the .NET hosting framework.
/// </summary>
public interface IFetcherSchedulerDueEvent : IHostedService
{
    /// <summary>
    /// Gets the unique identifier of the fetcher associated with this due event.
    /// </summary>
    string FetcherIdentifier { get; }

    /// <summary>
    /// Gets the unique identifier for this specific due event instance.
    /// </summary>
    Guid EventIdentifier { get; }

    /// <summary>
    /// Occurs when the scheduled due time is reached and the fetcher is ready for execution.
    /// </summary>
    event EventHandler<DueEventArgs>? TriggeredEvent;

    /// <summary>
    /// Occurs when the due event is cancelled or aborted before the scheduled due time is reached.
    /// </summary>
    event EventHandler<DueEventArgs>? AbortedEvent;
}
