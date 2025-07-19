using System;

namespace Deepo.Fetcher.Viewer.Models;

/// <summary>
/// Provides event data for grid model events, containing information about a grid row.
/// </summary>
public sealed class GridModelEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the grid model representing a row of data.
    /// </summary>
    public GridModel Row { get; set; }

    public GridModelEventArgs(GridModel row)
    {
        Row = row;
    }
}