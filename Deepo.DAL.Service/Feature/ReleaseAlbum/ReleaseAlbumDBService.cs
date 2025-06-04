using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Interfaces;
using Deepo.DAL.Service.LogMessage;
using Deepo.DAL.Service.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Service.Feature.ReleaseAlbum;

public class ReleaseAlbumDBService : IReleaseAlbumDBService
{
    private readonly ILogger _logger;

    private readonly DEEPOContext _dbContext;
    private readonly IAuthorDBService _authorDBService;

    public ReleaseAlbumDBService(DEEPOContext dbContext, IAuthorDBService authorDBService, ILogger<ReleaseAlbumDBService> logger)
    {
        _authorDBService = authorDBService;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<DatabaseServiceResult> Insert(AlbumModel item, CancellationToken cancellationToken)
    {
        DatabaseServiceResult result = new();

        if (item is null || item.Artists is null || Exists(item))
        {
            return result;
        }

        foreach (var author in item.Artists)
        {
            if (!_authorDBService.Exists(author))
            {
                DatabaseServiceResult innerResult = await _authorDBService.Insert(author, cancellationToken).ConfigureAwait(false);
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
                Author = _dbContext.Authors.First(x => x.Provider_Author_Identifier == artist.Provider_Identifier)
            });
        }

        Release newRelease = new()
        {
            Creation_Date = DateTime.Now,
            Modification_Date = DateTime.Now,
            Creation_User = "Auto",
            Name = item.Title ?? string.Empty,
            GUID = Guid.NewGuid().ToString(),
            Author_Releases = author_Releases,
            Type_Release = _dbContext.Type_Releases.First(x => x.Code == "VINYL") // todo
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
            Genre_Album? genre = _dbContext.Genre_Albums.FirstOrDefault(x => x.Name == genreStr);
            if (genre is not null)
            {
                genres.Add(genre);
            }
        }

        _dbContext.Release_Albums.Add(new Release_Album
        {
            Release = newRelease,
            Country = item.Country ?? string.Empty,
            Label = item.Label ?? string.Empty,
            Genre_Albums = genres
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
        }
        catch (Exception ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Release_Album), _dbContext?.Database?.GetDbConnection()?.ConnectionString, ex);
            throw;
        }

        return result;

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