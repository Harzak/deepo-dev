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

        Release_Album? album = _db.GetById(request.Id);

        if (album is null)
        {
            return result.WithFailure();
        }

        result.Content = new ReleaseVinylExDto()
        {
            Id = !string.IsNullOrEmpty(album.Release.GUID) ? Guid.Parse(album.Release.GUID) : Guid.Empty,
            Name = album.Release.Name ?? string.Empty,
            ReleaseDate = album.Release.Creation_Date,
            AuthorsNames = string.Join("-", album.Release.Author_Releases.Select(x => x.Author?.Name)) ?? string.Empty,
            Country = album.Country ?? string.Empty,
            Label = album.Label ?? string.Empty,
        };

        foreach (Asset_Release asset in album.Release.Asset_Releases)
        {
            result.Content.CoverUrl = asset.Asset?.Content_URL ?? string.Empty;
            result.Content.ThumbUrl = asset.Asset?.Content_Min_URL ?? string.Empty;
        }

        foreach (Genre_Album genre in album.Genre_Albums)
        {
            result.Content.Genres.Add(genre.Code, genre.Name);
        }

        foreach (var track in album.Tracklist_Albums)
        {
            result.Content.Tracklist.Add(new  TrackVinyl()
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

