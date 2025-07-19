namespace Deepo.Fetcher.Viewer.Models;

/// <summary>
/// Represents a data model for grid display with configurable columns and identification properties.
/// </summary>
public sealed class GridModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the grid item.
    /// </summary>
    public int ID { get; set; }
    
    /// <summary>
    /// Gets or sets the GUID-based identifier for the grid item.
    /// </summary>
    public string? IdentifierGuid { get; set; }
    
    /// <summary>
    /// Gets or sets the data for the first column of the grid.
    /// </summary>
    public string? Column1 { get; set; }
    
    /// <summary>
    /// Gets or sets the data for the second column of the grid.
    /// </summary>
    public string? Column2 { get; set; }
    
    /// <summary>
    /// Gets or sets the data for the third column of the grid.
    /// </summary>
    public string? Column3 { get; set; }
    
    /// <summary>
    /// Gets or sets the data for the fourth column of the grid.
    /// </summary>
    public string? Column4 { get; set; }
    
    /// <summary>
    /// Gets or sets the data for the fifth column of the grid.
    /// </summary>
    public string? Column5 { get; set; }
}