using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Dto;
using Deepo.Mediator.Query;
using Deepo.Framework.Results;
using MediatR;

namespace Deepo.Mediator.Handler;

/// <summary>
/// Handles the processing of queries to retrieve detailed information about a specific vinyl release.
/// </summary>
internal sealed class GetVinylReleaseHandler : IRequestHandler<GetVinylReleaseQuery, OperationResult<ReleaseVinylExDto>>
{
    private readonly IReleaseAlbumRepository _repository;

    public GetVinylReleaseHandler(IReleaseAlbumRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Processes the vinyl release query and returns detailed information including tracks, genres, and assets.
    /// </summary>
    public async Task<OperationResult<ReleaseVinylExDto>> Handle(GetVinylReleaseQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        OperationResult<ReleaseVinylExDto> result = new();

        if (request.Id == Guid.Empty)
        {
            return result.WithError("'Id' parameter must not be empty.");
        }

        Release_Album? vinylReleaseDB = await _repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (vinylReleaseDB is null)
        {
            return result.WithError($"no record found for parameter: 'id' = {request.Id}");
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

        return result.WithSuccess();
    }
}

