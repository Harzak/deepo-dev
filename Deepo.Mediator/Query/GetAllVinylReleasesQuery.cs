using Deepo.Dto;
using Deepo.Framework.Results;
using MediatR;

namespace Deepo.Mediator.Query;

public class GetAllVinylReleasesQuery : IRequest<OperationResultList<ReleaseVinylDto>>
{
    public string Market { get; set; }

    public GetAllVinylReleasesQuery(string market)
    {
        this.Market = market;
    }
}