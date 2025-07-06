using Deepo.Dto;
using Deepo.Framework.Results;
using MediatR;

namespace Deepo.Mediator.Query;

public class GetVinylReleaseQuery : IRequest<OperationResult<ReleaseVinylExDto>>
{
    public Guid Id { get; }

    public GetVinylReleaseQuery() : this(Guid.Empty)
    {

    }

    public GetVinylReleaseQuery(Guid id)
    {
        this.Id = id;
    }
}

