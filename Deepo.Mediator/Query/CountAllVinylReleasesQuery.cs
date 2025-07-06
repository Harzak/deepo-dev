using Deepo.Framework.Results;
using MediatR;

namespace Deepo.Mediator.Query;

public class CountAllVinylReleasesQuery : IRequest<OperationResult<int>>
{
    public string Market { get; set; }

    public CountAllVinylReleasesQuery(string market)
    {
        this.Market = market;
    }
}