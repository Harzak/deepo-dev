using Deepo.Fetcher.Library.Interfaces;
using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Workers.Scheduling;

/// <summary>
/// Factory class responsible for creating instances of <see cref="IFetcherSchedulerDueEvent"/>.
/// </summary>
public class FetcherSchedulerDueEventFactory : IFetcherSchedulerDueEventFactory
{
    private readonly IDateTimeFacade _dateTimeFacade;
    private readonly ILogger _logger;

    public FetcherSchedulerDueEventFactory(IDateTimeFacade dateTimeFacade, ILogger<FetcherSchedulerDueEventFactory> logger)
    {
        _dateTimeFacade = dateTimeFacade;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new instance of <see cref="IFetcherSchedulerDueEvent"/> for the specified fetcher and due time.
    /// </summary>
    /// <param name="fetcherIdentifier">The unique identifier of the fetcher that will be scheduled for execution.</param>
    /// <param name="dueAt">The UTC date and time when the fetcher should be executed.</param>
    /// <returns>
    /// A new <see cref="IFetcherSchedulerDueEvent"/> instance configured with the specified parameters
    /// and injected dependencies.
    /// </returns>
    public IFetcherSchedulerDueEvent Create(string fetcherIdentifier, DateTime dueAt)
    {
        return new FetcherSchedulerDueEvent(fetcherIdentifier, dueAt, _dateTimeFacade, _logger);
    }
}