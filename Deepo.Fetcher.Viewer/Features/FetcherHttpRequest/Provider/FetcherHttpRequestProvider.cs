using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Interfaces;
using Deepo.Fetcher.Viewer.Interfaces;
using Framework.Common.Data.SQLServer.ServiceBroker;
using Microsoft.Extensions.Logging;
using System;

namespace Deepo.Fetcher.Viewer.Features.FetcherHttpRequest.Provider;

internal sealed class FetcherHttpRequestProvider : IFetcherHttpRequestProvider
{
    public event EventHandler<string>? RowAdded;

    private readonly SQLListener _fetcherListener;
    private readonly IFetcherHttpRequestDBService  _httpRequestService;
    public FetcherHttpRequestProvider(string connectionString, string subcriptionRequest, IFetcherHttpRequestDBService httpRequestService, ILogger logger)
    {
         _httpRequestService = httpRequestService;
        _fetcherListener = new SQLListener(connectionString, subcriptionRequest, logger);
        _fetcherListener.StartListener();
        _fetcherListener.OnInsert += FetcherListener_OnInsert;
    }

    private void FetcherListener_OnInsert(object? sender, EventArgs e)
    {
        HttpRequest? lastrow = _httpRequestService.GetLast();
        if (lastrow is null)
        {
            return;
        }
        RowAdded?.Invoke(this, lastrow.RequestUri ?? "");
    }

    public void Dispose()
    {
        _fetcherListener.OnInsert -= FetcherListener_OnInsert;
        _fetcherListener?.Dispose();
    }
}

