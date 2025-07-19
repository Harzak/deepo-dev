using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Dto;
using Deepo.Framework.Results;
using Deepo.Mediator.Query;
using MediatR;

namespace Deepo.Mediator.Handler;

/// <summary>
/// Handles the processing of queries to retrieve all vinyl genres with optional filtering.
/// </summary>
internal sealed class GetAllVinylGenresHandler : IRequestHandler<GetAllVinylGenresQuery, OperationResultList<GenreDto>>
{
    private readonly IGenreAlbumRepository _repository;

    public GetAllVinylGenresHandler(IGenreAlbumRepository _repository)
    {
        this._repository = _repository;
    }

    /// <summary>
    /// Processes the genre query and returns a collection of available vinyl genres.
    /// </summary>
    public async Task<OperationResultList<GenreDto>> Handle(GetAllVinylGenresQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        OperationResultList<GenreDto> result = new()
        {
            Content = []
        };

        IReadOnlyCollection<Genre_Album> genres = await _repository.GetAllAsync(cancellationToken).ConfigureAwait(false);

        if (genres != null)
        {
            foreach (Genre_Album genre in genres)
            {
                result.Content.Add(new()
                {
                    Name = genre.Name ?? string.Empty,
                    Identifier = Guid.Parse(genre.Identifier),
                });
            }
        }
        result.WithSuccess();

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}