using Deepo.Framework.Results;
using MediatR;

namespace Deepo.Mediator.Query;

/// <summary>
/// Represents a query to count all vinyl releases for a specific market.
/// </summary>
public class CountAllVinylReleasesQuery : IRequest<OperationResult<int>>
{
    /// <summary>
    /// Gets or sets the market identifier for which to count vinyl releases.
    /// </summary>
    public string Market { get; set; }

    public CountAllVinylReleasesQuery(string market)
    {
        this.Market = market;
    }
}