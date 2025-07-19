namespace Deepo.Mediator.Query;

/// <summary>
/// Represents a query to retrieve vinyl releases for a specific market with pagination support.
/// </summary>
public class GetAllVinylReleasesWithPaginationQuery : GetAllVinylReleasesQuery
{
    /// <summary>
    /// Gets or sets the number of items to skip for pagination.
    /// </summary>
    public int Offset { get; set; }
    
    /// <summary>
    /// Gets or sets the maximum number of items to return.
    /// </summary>
    public int Limit { get; set; }

    public GetAllVinylReleasesWithPaginationQuery(string market) : base(market)
    {

    }
}