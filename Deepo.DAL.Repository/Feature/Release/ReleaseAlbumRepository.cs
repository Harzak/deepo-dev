using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.DAL.Repository.LogMessage;
using Deepo.DAL.Repository.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Feature.Release;

public class ReleaseAlbumRepository : IReleaseAlbumRepository
{
    private readonly ILogger _logger;

    private readonly DEEPOContext _dbContext;
    private readonly IAuthorRepository _authorRepository;
    private readonly IGenreAlbumRepository _genreAlbumRepository;

    public ReleaseAlbumRepository(DEEPOContext dbContext,
        IAuthorRepository authorRepository,
        IGenreAlbumRepository genreAlbumRepository,
        ILogger<ReleaseAlbumRepository> logger)
    {
        _dbContext = dbContext;
        _authorRepository = authorRepository;
        _genreAlbumRepository = genreAlbumRepository;
        _logger = logger;
    }

    public async Task<DatabaseOperationResult> InsertAsync(AlbumModel item, CancellationToken cancellationToken)
    {
        DatabaseOperationResult result = new();

        if (item is null || item.Artists is null || Exists(item))
        {
            return result;
        }

        foreach (var author in item.Artists)
        {
            if (!_authorRepository.Exists(author))
            {
                DatabaseOperationResult innerResult = await _authorRepository.Insert(author, cancellationToken).ConfigureAwait(false);
                if (!innerResult.IsSuccess)
                {
                    return innerResult;
                }
            }
        }

        List<Author_Release> author_Releases = [];
        foreach (var artist in item.Artists)
        {
            author_Releases.Add(new Author_Release
            {
                Author = _authorRepository.GetByProviderIdentifier(artist.Provider_Identifier)
            });
        }

        Models.Release newRelease = new()
        {
            Name = item.Title ?? string.Empty,
            Release_Date_UTC = item.ReleaseDateUTC ?? DateTime.UtcNow,
            GUID = Guid.NewGuid().ToString(),
            Author_Releases = author_Releases,
            Type_Release = _dbContext.Type_Releases.First(x => x.Code == "VINYL"), // todo
            Creation_User = "Auto",
            Creation_Date = DateTime.UtcNow,
            Modification_Date = DateTime.UtcNow
        };


        if (!string.IsNullOrEmpty(item.ThumbURL))
        {
            newRelease.Asset_Releases =
            [
                new Asset_Release()
                {
                    Asset = new()
                    {
                        Content_Min_URL = item.ThumbURL,
                        Content_URL = item.CoverURL ?? string.Empty,
                    }
                }
            ];
        }

        foreach (KeyValuePair<string, string> providerIdentifier in item.ProvidersIdentifier)
        {
            _dbContext.Provider_Releases.Add(new Models.Provider_Release()
            {
                Provider = _dbContext.Providers.First(x => x.Code == providerIdentifier.Key),
                Provider_Release_Identifier = providerIdentifier.Value,
                Release = newRelease
            });
        }

        List<Genre_Album> genres = [];
        foreach (var genreStr in item.Genres)
        {
            if (_genreAlbumRepository.TryFindGenre(genreStr, out Genre_Album genre))
            {
                genres.Add(genre);
            }
            else
            {
                await _genreAlbumRepository.Insert(genreStr, cancellationToken).ConfigureAwait(false);
                if (_genreAlbumRepository.TryFindGenre(genreStr, out genre))
                {
                    genres.Add(genre);
                }
            }
        }

        List<Tracklist_Album> tracklist = [];
        foreach (TrackModel track in item.Tracklist)
        {
            tracklist.Add(new Tracklist_Album
            {
                Track_Album = new Track_Album
                {
                    Title = track.Title ?? string.Empty,
                    Duration = track.Duration,
                    Position = track.Position
                }
            });
        }

        _dbContext.Release_Albums.Add(new Release_Album
        {
            Release = newRelease,
            Country = item.Country ?? string.Empty,
            Market = "FR",
            Label = item.Label ?? string.Empty,
            Genre_Albums = genres,
            Tracklist_Albums = tracklist
        });

        try
        {
            int rowAffected = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            result.IsSuccess = rowAffected >= 0;
            result.RowAffected = rowAffected;
            return result;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Release_Album), _dbContext?.Database?.GetDbConnection()?.ConnectionString, ex);
            return result;
        }
        catch (Exception ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Release_Album), _dbContext?.Database?.GetDbConnection()?.ConnectionString, ex);
            throw;
        }
    }

    public int Count(string market)
    {
        return _dbContext.V_LastVinylReleases.Count();
    }

    public bool Exists(AlbumModel item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        foreach (var providerItem in item.ProvidersIdentifier)
        {
            return _dbContext.Provider_Releases
                .Any(x => x.Provider.Code == providerItem.Key
                    && x.Provider_Release_Identifier == providerItem.Value);

        }
        return false;
    }

    public V_LastVinylRelease? GetLast()
    {
        return _dbContext.V_LastVinylReleases
            .OrderBy(x => x.Release_ID)
            .LastOrDefault();
    }

    public IReadOnlyCollection<V_LastVinylRelease> GetAll()
    {
        return _dbContext.V_LastVinylReleases
            .ToList()
            .AsReadOnly();
    }

    public IReadOnlyCollection<V_LastVinylRelease> GetAll(string market, int offset, int limit)
    {
        return _dbContext.V_LastVinylReleases
            .OrderByDescending(x => x.Release_ID)
            .Skip(offset)
            .Take(limit)
            .ToList()
            .AsReadOnly();
    }

    public Release_Album? GetById(Guid id)
    {
        return _dbContext.Release_Albums
            .Include(x => x.Tracklist_Albums)
            .ThenInclude(x => x.Track_Album)
            .Include(x => x.Genre_Albums)
            .Include(x => x.Release)
            .ThenInclude(x => x.Asset_Releases)
            .ThenInclude(x => x.Asset)
            .Include(x => x.Release)
            .ThenInclude(x => x.Author_Releases)
            .ThenInclude(x => x.Author)
            .FirstOrDefault(x => x.Release != null
                            && x.Release.GUID == id.ToString());
    }
}