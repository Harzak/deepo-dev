using Deepo.Fetcher.WPF.Features.FetcherGrid.Model;
using Framework.Common.Data.SQLServer.ServiceBroker;
using Microsoft.Extensions.Logging;
using System;

namespace Deepo.Fetcher.WPF.Features.FetcherGrid.Provider
{
    internal class FetcherMovieGridProvider : IFetcherGridProvider, IDisposable
    {
        public event EventHandler<GridModelEventArgs>? RowAdded;

        private readonly SQLListener _fetcherListener;

        internal FetcherMovieGridProvider(string connectionString, string subcriptionRequest, ILogger logger)
        {
            _fetcherListener = new SQLListener(connectionString, subcriptionRequest, logger);
            _fetcherListener.StartListener();
            _fetcherListener.OnInsert += FetcherListener_OnInsert;
        }

        private void FetcherListener_OnInsert(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _fetcherListener.OnInsert -= FetcherListener_OnInsert;
            _fetcherListener?.Dispose();
        }
    }
}
