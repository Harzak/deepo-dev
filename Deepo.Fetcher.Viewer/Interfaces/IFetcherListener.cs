using Deepo.Fetcher.Viewer.Features.FetcherGrid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Viewer.Interfaces;

internal interface IFetcherListener : IDisposable
{
    event EventHandler<string>? HttpRequestRowAdded;
    event EventHandler<GridModelEventArgs>? VinylReleaseRowAdded;

    void StartListener();
}
