using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Dto;
using Deepo.Framework.Results;
using Deepo.Mediator.Query;
using MediatR;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Deepo.Mediator.Tests")]

namespace Deepo.Mediator.Handler;

/// <summary>
/// Handles the processing of queries to retrieve vinyl releases with pagination support.
/// </summary>
internal sealed class GetAllVinylReleasesHandler : IRequestHandler<GetAllVinylReleasesWithPaginationQuery, OperationResultList<ReleaseVinylDto>>
{
    private readonly IReleaseAlbumRepository _repository;

    public GetAllVinylReleasesHandler(IReleaseAlbumRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Processes the paginated vinyl releases query and returns a collection of vinyl releases with associated metadata.
    /// </summary>
    public async Task<OperationResultList<ReleaseVinylDto>> Handle(GetAllVinylReleasesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        OperationResultList<ReleaseVinylDto> result = new()
        {
            Content = []
        };

        if (request.Offset < 0)
        {
            return result.WithError("'Offset' parameter must be greater than 0.");
        }
        if (request.Limit < 0)
        {
            return result.WithError("'Limit' parameter must be greater than 0.");
        }
        if (string.IsNullOrEmpty(request.Market?.Trim()))
        {
            return result.WithError("'Market' parameter must not be empty.");
        }

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
                if (!string.IsNullOrEmpty(vinylReleaseDB.ArtistsNames))
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

