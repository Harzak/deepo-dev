using Deepo.Dto;
using Deepo.Framework.Results;
using MediatR;

namespace Deepo.Mediator.Query;

/// <summary>
/// Represents a query to retrieve detailed information about a specific vinyl release by its identifier.
/// </summary>
public class GetVinylReleaseQuery : IRequest<OperationResult<ReleaseVinylExDto>>
{
    /// <summary>
    /// Gets the unique identifier of the vinyl release to retrieve.
    /// </summary>
    public Guid Id { get; }

    public GetVinylReleaseQuery() : this(Guid.Empty)
    {

    }

    public GetVinylReleaseQuery(Guid id)
    {
        this.Id = id;
    }
}

