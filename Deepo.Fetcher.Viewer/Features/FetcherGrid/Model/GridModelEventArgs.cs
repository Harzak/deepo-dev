using System;

namespace Deepo.Fetcher.Viewer.Features.FetcherGrid.Model;

public class GridModelEventArgs : EventArgs
{
    public GridModel Row { get; set; }

    public GridModelEventArgs(GridModel row)
    {
        Row = row;
    }
}