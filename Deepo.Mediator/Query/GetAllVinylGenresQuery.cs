using Deepo.Dto;
using Framework.Common.Utils.Result;
using MediatR;

namespace Deepo.Mediator.Query;

public class GetAllVinylGenresQuery : IRequest<OperationResultList<GenreDto>>
{
    public string? Filter { get; set; }

    public GetAllVinylGenresQuery(string? filter)
    {
        this.Filter = filter;
    }
}
