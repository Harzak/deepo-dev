using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Workers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Deepo.Fetcher.Library.Fetcher.Movie;

/// <summary>
/// Movie-specific fetcher implementation that processes movie data.
/// </summary>
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

    /// <summary>
    /// Executes the movie fetcher's main operation asynchronously.
    /// </summary>
    protected override Task ExecuteInternalAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException("Movie fetching logic is not implemented yet.");
    }

    protected override void ForcedStop()
    {
        Dispose();
    }

    public override void Dispose()
    {
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}

