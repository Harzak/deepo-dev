using Deepo.Dto;
using Deepo.Framework.Results;
using MediatR;

namespace Deepo.Mediator.Query;

/// <summary>
/// Represents a query to retrieve all vinyl releases for a specific market.
/// </summary>
public class GetAllVinylReleasesQuery : IRequest<OperationResultList<ReleaseVinylDto>>
{
    /// <summary>
    /// Gets or sets the market identifier for which to retrieve vinyl releases.
    /// </summary>
    public string Market { get; set; }

    public GetAllVinylReleasesQuery(string market)
    {
        this.Market = market;
    }
}