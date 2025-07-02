using System;

namespace Deepo.Fetcher.Viewer.Models;

public sealed class GridModelEventArgs : EventArgs
{
    public GridModel Row { get; set; }

    public GridModelEventArgs(GridModel row)
    {
        Row = row;
    }
}