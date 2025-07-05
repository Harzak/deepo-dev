using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Dto;
using Deepo.Framework.Results;
using Deepo.Mediator.Query;
using MediatR;

namespace Deepo.Mediator.Handler;

internal sealed class GetAllVinylReleasesHandler : IRequestHandler<GetAllVinylReleasesWithPaginationQuery, OperationResultList<ReleaseVinylDto>>
{
    private readonly IReleaseAlbumRepository _repository;

    public GetAllVinylReleasesHandler(IReleaseAlbumRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResultList<ReleaseVinylDto>> Handle(GetAllVinylReleasesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        OperationResultList<ReleaseVinylDto> result = new()
        {
            Content = []
        };

        IReadOnlyCollection<V_LastVinylRelease> allVinylReleasesDB = await _repository.GetAllAsync(request.Market, 
                                                                                                   request.Offset, 
                                                                                                   request.Limit, 
                                                                                                   cancellationToken).ConfigureAwait(false);

        if (allVinylReleasesDB != null)
        {
            foreach (V_LastVinylRelease vinylReleaseDB in allVinylReleasesDB)
            {
                ReleaseVinylDto dto = new()
                {
                    Id = !string.IsNullOrEmpty(vinylReleaseDB.ReleasGUID) ? Guid.Parse(vinylReleaseDB.ReleasGUID) : Guid.Empty,
                    Name = vinylReleaseDB.AlbumName ?? string.Empty,
                    ReleaseDate = vinylReleaseDB.Release_Date_UTC,
                    Market = vinylReleaseDB.Market ?? string.Empty,
                    CoverUrl = vinylReleaseDB.Cover_URL ?? string.Empty
                };
                if (!string.IsNullOrEmpty(vinylReleaseDB.GenresIdentifier))
                {
                    string[] genresDB = vinylReleaseDB.GenresIdentifier.Split(';');
                    foreach (string genreDB in genresDB)
                    {
                        dto.Genres.Add(new GenreDto()
                        {
                            Identifier = Guid.Parse(genreDB)
                        });
                    }
                }
                if(!string.IsNullOrEmpty(vinylReleaseDB.ArtistsNames))
                {
                    string[] artistsNames = vinylReleaseDB.ArtistsNames.Split(';');
                    foreach (string artistName in artistsNames)
                    {
                        dto.AuthorsNames.Add(artistName);
                    }
                }
                result.Content.Add(dto);
            }
        }
        result.WithSuccess();

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}

