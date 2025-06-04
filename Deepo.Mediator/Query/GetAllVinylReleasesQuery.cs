using Deepo.Dto;
using Framework.Common.Utils.Result;
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
