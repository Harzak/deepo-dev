using System;

namespace Deepo.Fetcher.Viewer.Models;

internal sealed class GridModelEventArgs : EventArgs
{
    public GridModel Row { get; set; }

    public GridModelEventArgs(GridModel row)
    {
        Row = row;
    }
}