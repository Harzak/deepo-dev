using Deepo.Fetcher.Viewer.Features.FetcherGrid.Model;
using System;

namespace Deepo.Fetcher.Viewer.Interfaces;

internal interface IFetcherGridProvider : IDisposable
{
    event EventHandler<GridModelEventArgs> RowAdded;
}