using Deepo.Framework.Interfaces;

namespace Deepo.Framework.Web.Handler;

public class RateLimitHandler : DelegatingHandler
{
    protected virtual int Rate { get; set; }
    protected virtual TimeSpan Time { get; set; }

    private readonly ITimeProvider _timeProvider;
    private readonly List<DateTimeOffset> _requestHistory;

    public RateLimitHandler(int rate, TimeSpan time, ITimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        _requestHistory = [];
        Rate = rate;
        Time = time;
    }

    public RateLimitHandler(int rate, TimeSpan time, ITimeProvider timeProvider, HttpMessageHandler innerHandler) : base(innerHandler)
    {
        _timeProvider = timeProvider;
        _requestHistory = [];
        Rate = rate;
        Time = time;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        DateTimeOffset dateNow = _timeProvider.DateTimeOffsetNow();

        lock (_requestHistory)
        {
            _requestHistory.Add(dateNow);

            while (_requestHistory.Count > Rate)
            {
                _requestHistory.RemoveAt(0);
            }
        }

        await LimitDelay(dateNow).ConfigureAwait(false);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private async Task LimitDelay(DateTimeOffset dateNow)
    {
        if (_requestHistory.Count < Rate)
            return;

        DateTimeOffset limit = dateNow.Add(-Time);

        DateTimeOffset lastCall = _timeProvider.DateTimeOffsetMinValue();
        bool shouldLock = false;

        lock (_requestHistory)
        {
            lastCall = _requestHistory.FirstOrDefault();
            shouldLock = _requestHistory.Count(x => x >= limit) >= Rate;
        }

        TimeSpan delayTime = shouldLock && (lastCall > _timeProvider.DateTimeOffsetMinValue()) ? (lastCall - limit) : TimeSpan.Zero;

        if (delayTime > TimeSpan.Zero)
        {
            await Task.Delay(delayTime).ConfigureAwait(false);
        }
    }
}
