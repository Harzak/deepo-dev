using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Workers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Deepo.Fetcher.Library.Fetcher.Movie;

public class FetcherMovie : CancellableWorker
{
    private readonly FetcherOptions _fetcherOptions;

    public FetcherMovie(IOptions<FetcherOptions> config,
        ILogger logger)
    : base(logger)
    {
        ArgumentNullException.ThrowIfNull(config);
        _fetcherOptions = config.Value;
        Name = _fetcherOptions.FetcherMovieName;
        ID = Guid.Parse(_fetcherOptions.FetcherMovieId);
    }

    protected override bool CanStop()
    {
        return true;
    }

    protected override Task ExecuteInternalAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    protected override void ForcedStop()
    {
        Dispose();
        //do some action on critical thing
    }
    public override void Dispose()
    {
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}

