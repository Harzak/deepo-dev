using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Viewer.Constants;
using Deepo.Fetcher.Viewer.Interfaces;
using Deepo.Fetcher.Viewer.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Viewer.Features.Listener;

internal sealed class FetcherListener : IFetcherListener
{
    private readonly SQLListener _releaseVinyListener;
    private readonly SQLListener _requestListener;
    private readonly SQLListener _fetcherListener;

    private readonly IReleaseAlbumRepository _releaseAlbumRepository;
    private readonly IFetcherHttpRequestRepository _httpRequestRepository;
    private readonly IFetcherExecutionRepository _fetcherExecutionRepository;

    public event EventHandler<HttpRequestLogEventArgs>? HttpRequestLogRowAdded;
    public event EventHandler<GridModelEventArgs>? VinylReleaseRowAdded;
    public event EventHandler<FetcherExecutionEventArgs>? FetcherExecutionRowAdded;


    public FetcherListener(string connectionString,
        IReleaseAlbumRepository releaseAlbumRepository,
        IFetcherHttpRequestRepository httpRequestRepository,
        IFetcherExecutionRepository fetcherExecutionRepository,
        ILogger logger)
    {
        _httpRequestRepository = httpRequestRepository;
        _releaseAlbumRepository = releaseAlbumRepository;
        _fetcherExecutionRepository = fetcherExecutionRepository;
        _releaseVinyListener = new SQLListener(connectionString, Queries.VinyleSubscriptionQuery, logger);
        _requestListener = new SQLListener(connectionString, Queries.HttpRequestSubscriptionQuery, logger);
        _fetcherListener = new SQLListener(connectionString, Queries.FectherExecutionSubscriptionQuery, logger);
    }

    public void StartListener()
    {
        _releaseVinyListener.OnInsert += OnInsertReleaseVinylAsync;
        _releaseVinyListener.StartListener();

        _requestListener.OnInsert += OnInsertHttpRequestLogAsync;
        _requestListener.StartListener();

        _fetcherListener.OnInsert += OnInsertFetcherExecutionAsync;
        _fetcherListener.StartListener();
    }

    private async void OnInsertReleaseVinylAsync(object? sender, EventArgs e)
    {
        V_LastVinylRelease? lastrow = await _releaseAlbumRepository.GetLastAsync().ConfigureAwait(false);
        if (lastrow != null)
        {
            GridModel model = new()
            {
                ID = lastrow.Release_ID,
                IdentifierGuid = lastrow.ReleasGUID,
                Column1 = lastrow.AlbumName,
                Column2 = lastrow.ArtistsNames,
                Column3 = lastrow.Release_Date_UTC.ToString(CultureInfo.CurrentCulture)
            };
            VinylReleaseRowAdded?.Invoke(this, new GridModelEventArgs(model));
        }
    }

    private async void OnInsertHttpRequestLogAsync(object? sender, EventArgs e)
    {
        HttpRequest? lastrow = await _httpRequestRepository.GetLastAsync().ConfigureAwait(false);
        if (lastrow != null)
        {
            HttpRequestLogRowAdded?.Invoke(this, new HttpRequestLogEventArgs(lastrow.RequestUri ?? "n/a"));
        }
    }

    private async void OnInsertFetcherExecutionAsync(object? sender, EventArgs e)
    {
        V_FetchersLastExecution? lastFetcher = await _fetcherExecutionRepository.GetLastFetcherExecutionAsync().ConfigureAwait(false);
        if (lastFetcher != null)
        {
            FetcherExecutionEventArgs args = new(lastFetcher.Fetcher_GUID, lastFetcher.StartedAt ?? DateTime.UtcNow);
            FetcherExecutionRowAdded?.Invoke(this, args);
        }
    }

    public void Dispose()
    {
        if (_releaseVinyListener != null)
        {
            _releaseVinyListener.OnInsert -= OnInsertReleaseVinylAsync;
            _releaseVinyListener.Dispose();
        }
        if (_requestListener != null)
        {
            _requestListener.OnInsert -= OnInsertHttpRequestLogAsync;
            _requestListener.Dispose();
        }
        if (_fetcherListener != null)
        {
            _fetcherListener.OnInsert -= OnInsertFetcherExecutionAsync;
            _fetcherListener.Dispose();
        }
    }
}