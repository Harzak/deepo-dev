using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Viewer.Constants;
using Deepo.Fetcher.Viewer.Interfaces;
using Deepo.Fetcher.Viewer.Models;
using Framework.Common.Data.SQLServer.ServiceBroker;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace Deepo.Fetcher.Viewer.Features.Listener;

internal sealed class FetcherListener : IFetcherListener
{
    private readonly SQLListener _releaseVinyListener;
    private readonly SQLListener _requestListener;

    private readonly IReleaseAlbumRepository _releaseAlbumRepository;
    private readonly IFetcherHttpRequestRepository _httpRequestRepository;

    public event EventHandler<string>? HttpRequestRowAdded;
    public event EventHandler<GridModelEventArgs>? VinylReleaseRowAdded;

    public FetcherListener(string connectionString, 
        IReleaseAlbumRepository releaseAlbumRepository,
        IFetcherHttpRequestRepository httpRequestRepository,
        ILogger logger)
    {
        _httpRequestRepository = httpRequestRepository;
        _releaseAlbumRepository = releaseAlbumRepository;
        _releaseVinyListener = new SQLListener(connectionString, Queries.VinyleSubscriptionQuery, logger);
        _requestListener = new SQLListener(connectionString, Queries.HttpRequestSubscriptionQuery, logger);
    }

    public void StartListener()
    {
        _releaseVinyListener.StartListener();
        _releaseVinyListener.OnInsert += OnInsertReleaseVinyl;

        _requestListener.StartListener();
        _requestListener.OnInsert += OnInsertHttpRequest;
    }

    private void OnInsertReleaseVinyl(object? sender, EventArgs e)
    {
        V_LastVinylRelease? lastrow = _releaseAlbumRepository.GetLast();
        if (lastrow is null)
        {
            return;
        }
        GridModel model = new()
        {
            ID = lastrow.Release_ID ?? -1,
            GUID_ID = lastrow.ReleasGUID,
            Column1 = lastrow.AlbumName,
            Column2 = lastrow.ArtistsNames,
            Column3 = lastrow.Release_Date_UTC.ToString(CultureInfo.CurrentCulture)
        };
        VinylReleaseRowAdded?.Invoke(this, new GridModelEventArgs(model));
    }

    private void OnInsertHttpRequest(object? sender, EventArgs e)
    {
        HttpRequest? lastrow = _httpRequestRepository.GetLast();
        if (lastrow is null)
        {
            return;
        }
        HttpRequestRowAdded?.Invoke(this, lastrow.RequestUri ?? "");
    }

    public void Dispose()
    {
        if (_releaseVinyListener != null)
        {
            _releaseVinyListener.OnInsert -= OnInsertReleaseVinyl;
            _releaseVinyListener.Dispose();
        }
        if (_requestListener != null)
        {
            _requestListener.OnInsert -= OnInsertHttpRequest;
            _requestListener.Dispose();
        }
    }
}