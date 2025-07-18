using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Interfaces;

/// <summary>
/// Defines the contract for a factory that creates instances of <see cref="IFetcherSchedulerDueEvent"/>.
/// This factory interface provides a standardized way to create due event instances with the necessary
/// dependencies and configuration for scheduled fetcher task execution.
/// </summary>
public interface IFetcherSchedulerDueEventFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="IFetcherSchedulerDueEvent"/> for the specified fetcher and due time.
    /// </summary>
    IFetcherSchedulerDueEvent Create(string fetcherIdentifier, DateTime dueAt);
}
