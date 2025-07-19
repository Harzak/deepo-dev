using Deepo.Dto;
using Deepo.Framework.Results;
using MediatR;

namespace Deepo.Mediator.Query;

/// <summary>
/// Represents a query to retrieve all vinyl genres with optional filtering capabilities.
/// </summary>
public class GetAllVinylGenresQuery : IRequest<OperationResultList<GenreDto>>
{
    /// <summary>
    /// Gets or sets the optional filter to apply when retrieving genres.
    /// </summary>
    public string? Filter { get; set; }

    public GetAllVinylGenresQuery(string? filter)
    {
        this.Filter = filter;
    }
}
