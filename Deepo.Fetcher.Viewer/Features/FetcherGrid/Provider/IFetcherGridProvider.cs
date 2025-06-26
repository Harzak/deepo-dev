using Deepo.Fetcher.WPF.Features.FetcherGrid.Model;
using System;

namespace Deepo.Fetcher.WPF.Features.FetcherGrid.Provider
{
    internal interface IFetcherGridProvider : IDisposable
    {
        event EventHandler<GridModelEventArgs> RowAdded;
    }
}
