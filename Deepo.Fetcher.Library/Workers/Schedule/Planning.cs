using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Library.Workers.Schedule;

public abstract class Planning : IPlanning
{
    protected ITimeProvider TimeProvider { get; set; }
    protected DateTime? NextStart { get; set; }
    public DateTime? DateNextStart { get => NextStart; }

    private readonly ILogger _logger;

    protected Planning(ITimeProvider TimeProvider, ILogger logger)
    {
        _logger = logger;
        this.TimeProvider = TimeProvider;
    }

    public bool ShouldStart()
    {
        bool shouldStart = false;
        DateTime dateNow = TimeProvider.DateTimeUTCNow();

        if (!NextStart.HasValue || (NextStart.HasValue && NextStart.Value < dateNow))
        {
            shouldStart = ShouldStart(dateNow);

            if (shouldStart)
            {
                GetNext(dateNow);
            }
        }
        return shouldStart;
    }

    protected abstract bool ShouldStart(DateTime startDate);

    protected abstract void GetNext(DateTime datetime);
}
