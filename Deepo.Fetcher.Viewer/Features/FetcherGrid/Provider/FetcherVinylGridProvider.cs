using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Interfaces;
using Deepo.Fetcher.Viewer.Interfaces;
using Deepo.Fetcher.Viewer.Features.FetcherGrid.Model;
using Framework.Common.Data.SQLServer.ServiceBroker;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace Deepo.Fetcher.Viewer.Features.FetcherGrid.Provider;

internal sealed class FetcherVinylGridProvider : IFetcherGridProvider, IDisposable
{
    public event EventHandler<GridModelEventArgs>? RowAdded;

    private readonly SQLListener _fetcherListener;
    private readonly IReleaseAlbumDBService _releaseAlbumDBService;
    public FetcherVinylGridProvider(string connectionString, string subcriptionRequest, IReleaseAlbumDBService releaseAlbumDBService, ILogger logger)
    {
        _releaseAlbumDBService = releaseAlbumDBService;
        _fetcherListener = new SQLListener(connectionString, subcriptionRequest, logger);
        _fetcherListener.StartListener();
        _fetcherListener.OnInsert += FetcherListener_OnInsert;
    }

    private void FetcherListener_OnInsert(object? sender, EventArgs e)
    {
        V_LastVinylRelease? lastrow = _releaseAlbumDBService.GetLast();
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
        RowAdded?.Invoke(this, new GridModelEventArgs(model));
    }

    public void Dispose()
    {
        _fetcherListener.OnInsert -= FetcherListener_OnInsert;
        _fetcherListener?.Dispose();
    }
}
