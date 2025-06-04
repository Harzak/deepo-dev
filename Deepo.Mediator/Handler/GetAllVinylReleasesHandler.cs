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

        IReadOnlyCollection<V_LastVinylRelease> releases = _db.GetAll(request.Market, request.Offset, request.Limit);

        if (releases != null)
        {
            foreach (V_LastVinylRelease release in releases)
            {
                result.Content.Add(new()
                {
                    Id = !string.IsNullOrEmpty(release.ReleasGUID) ? Guid.Parse(release.ReleasGUID) : Guid.Empty,
                    Name = release.AlbumName ?? string.Empty,
                    ReleaseDate = release.Creation_Date ?? DateTime.MinValue,
                    AuthorsNames = release.ArtistsNames ?? string.Empty,
                    ThumbUrl = release.Thumb_URL ?? string.Empty,
                    CoverUrl = release.Cover_URL ?? string.Empty
                });
            }
        }
        result.WithSuccess();

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}

