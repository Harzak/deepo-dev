using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Interfaces;
using Deepo.Dto;
using Deepo.Mediator.Query;
using Framework.Common.Utils.Result;
using MediatR;

namespace Deepo.Mediator.Handler;

internal sealed class GetAllVinylReleasesHandler : IRequestHandler<GetAllVinylReleasesWithPaginationQuery, OperationResultList<ReleaseVinylDto>>
{
    private readonly IReleaseAlbumDBService _db;

    public GetAllVinylReleasesHandler(IReleaseAlbumDBService dbService)
    {
        _db = dbService;
    }

    public async Task<OperationResultList<ReleaseVinylDto>> Handle(GetAllVinylReleasesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        OperationResultList<ReleaseVinylDto> result = new()
        {
            Content = []
        };

        IReadOnlyCollection<V_LastVinylRelease> allVinylReleasesDB = _db.GetAll(request.Market, request.Offset, request.Limit);

        if (allVinylReleasesDB != null)
        {
            foreach (V_LastVinylRelease vinylReleaseDB in allVinylReleasesDB)
            {
                ReleaseVinylDto dto = new()
                {
                    Id = !string.IsNullOrEmpty(vinylReleaseDB.ReleasGUID) ? Guid.Parse(vinylReleaseDB.ReleasGUID) : Guid.Empty,
                    Name = vinylReleaseDB.AlbumName ?? string.Empty,
                    ReleaseDate = vinylReleaseDB.Creation_Date ?? DateTime.MinValue,
                    AuthorsNames = vinylReleaseDB.ArtistsNames ?? string.Empty,
                    ThumbUrl = vinylReleaseDB.Thumb_URL ?? string.Empty,
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
                result.Content.Add(dto);
            }
        }
        result.WithSuccess();

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}

