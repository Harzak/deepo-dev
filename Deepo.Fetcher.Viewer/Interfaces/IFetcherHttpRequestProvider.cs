using System;

namespace Deepo.Fetcher.Viewer.Interfaces;

internal interface IFetcherHttpRequestProvider : IDisposable
{
    event EventHandler<string> RowAdded;
}