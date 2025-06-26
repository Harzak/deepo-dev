using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Interfaces;
using Deepo.Dto;
using Deepo.Mediator.Query;
using Framework.Common.Utils.Result;
using MediatR;

namespace Deepo.Mediator.Handler;

internal sealed class GetAllVinylGenresHandler : IRequestHandler<GetAllVinylGenresQuery, OperationResultList<GenreDto>>
{
    private readonly IGenreAlbumService _db;

    public GetAllVinylGenresHandler(IGenreAlbumService dbService)
    {
        _db = dbService;
    }

    public async Task<OperationResultList<GenreDto>> Handle(GetAllVinylGenresQuery request, CancellationToken cancellationToken)
    {
        OperationResultList<GenreDto> result = new()
        {
            Content = []
        };

        IReadOnlyCollection<Genre_Album> genres = _db.GetAll();

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