using Deepo.Dto;
using Framework.Common.Utils.Result;
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

