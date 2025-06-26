using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Interfaces;
using Deepo.Dto;
using Deepo.Mediator.Query;
using Framework.Common.Utils.Result;
using MediatR;

namespace Deepo.Mediator.Handler;

internal sealed class GetVinylReleaseHandler : IRequestHandler<GetVinylReleaseQuery, OperationResult<ReleaseVinylExDto>>
{
    private readonly IReleaseAlbumDBService _db;

    public GetVinylReleaseHandler(IReleaseAlbumDBService db)
    {
        _db = db;
    }

    public async Task<OperationResult<ReleaseVinylExDto>> Handle(GetVinylReleaseQuery request, CancellationToken cancellationToken)
    {

        OperationResult<ReleaseVinylExDto> result = new();

        Release_Album? vinylReleaseDB = _db.GetById(request.Id);

        if (vinylReleaseDB is null)
        {
            return result.WithFailure();
        }

        result.Content = new ReleaseVinylExDto()
        {
            Id = !string.IsNullOrEmpty(vinylReleaseDB.Release.GUID) ? Guid.Parse(vinylReleaseDB.Release.GUID) : Guid.Empty,
            Name = vinylReleaseDB.Release.Name ?? string.Empty,
            ReleaseDate = vinylReleaseDB.Release.Release_Date_UTC,
            Country = vinylReleaseDB.Country ?? string.Empty,
            Market = vinylReleaseDB.Market ?? string.Empty,
            Label = vinylReleaseDB.Label ?? string.Empty,
        };

        foreach (Asset_Release asset in vinylReleaseDB.Release.Asset_Releases)
        {
            result.Content.CoverUrl = asset.Asset?.Content_URL ?? string.Empty;
        }

        foreach (Genre_Album genre in vinylReleaseDB.Genre_Albums)
        {
            result.Content.Genres.Add(new GenreDto()
            {
                Name = genre.Name,
                Identifier = Guid.Parse(genre.Identifier)
            });
        }

        foreach (string? artistName in vinylReleaseDB.Release.Author_Releases.Select(x => x.Author?.Name))
        {
            if (!string.IsNullOrEmpty(artistName))
            {
                result.Content.AuthorsNames.Add(artistName);
            }
        }

        foreach (var track in vinylReleaseDB.Tracklist_Albums)
        {
            result.Content.Tracklist.Add(new TrackVinyl()
            {
                Title = track.Track_Album.Title ?? string.Empty,
                Duration = track.Track_Album.Duration,
                Position = track.Track_Album.Position
            });
        }

        result.WithSuccess();

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}

