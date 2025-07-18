using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.DAL.Repository.LogMessage;
using Deepo.DAL.Repository.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Feature.Release;

/// <summary>
/// Repository for managing album release data including insertion, existence checks, and retrieval operations.
/// </summary>
public class ReleaseAlbumRepository : IReleaseAlbumRepository
{
    private readonly ILogger _logger;
    private readonly IDbContextFactory<DEEPOContext> _contextFactory;
    private readonly IAuthorRepository _authorRepository;
    private readonly IGenreAlbumRepository _genreAlbumRepository;

    public ReleaseAlbumRepository(IDbContextFactory<DEEPOContext> contextFactory,
        IAuthorRepository authorRepository,
        IGenreAlbumRepository genreAlbumRepository,
        ILogger<ReleaseAlbumRepository> logger)
    {
        _contextFactory = contextFactory;
        _authorRepository = authorRepository;
        _genreAlbumRepository = genreAlbumRepository;
        _logger = logger;
    }

    /// <summary>
    /// Inserts a complete album with associated artists, genres, and tracks into the database.
    /// </summary>
    public async Task<DatabaseOperationResult> InsertAsync(AlbumModel item, CancellationToken cancellationToken = default)
    {
        DatabaseOperationResult result = new();

        if (item is null || item.Artists is null)
        {
            return result;
        }

        bool exists = await ExistsAsync(item, cancellationToken).ConfigureAwait(false);
        if (exists)
        {
            return result;
        }

        foreach (var author in item.Artists)
        {
            bool authorExists = await _authorRepository.ExistsAsync(author, cancellationToken).ConfigureAwait(false);
            if (!authorExists)
            {
                DatabaseOperationResult innerResult = await _authorRepository.Insert(author, cancellationToken).ConfigureAwait(false);
                if (!innerResult.IsSuccess)
                {
                    return innerResult;
                }
            }
        }

        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        Type_Release typeRelease = await context.Type_Releases.FirstAsync(x => x.Code == "VINYL", cancellationToken).ConfigureAwait(false);

        Models.Release newRelease = new()
        {
            Name = item.Title ?? string.Empty,
            Release_Date_UTC = item.ReleaseDateUTC ?? DateTime.UtcNow,
            GUID = Guid.NewGuid().ToString(),
            Type_Release_ID = typeRelease.Type_Release_ID,
            Creation_User = "Auto",
            Creation_Date = DateTime.UtcNow,
            Modification_Date = DateTime.UtcNow
        };

        foreach (IAuthor artist in item.Artists)
        {
            Models.Author? authorModel = await _authorRepository.GetByProviderIdentifierAsync(artist.Provider_Identifier, cancellationToken).ConfigureAwait(false);
            if (authorModel != null)
            {
                newRelease.Author_Releases.Add(new Author_Release()
                {
                    Author_ID = authorModel.Author_ID,
                });
            }
        }


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
            context.Provider_Releases.Add(new Provider_Release()
            {
                Provider = await context.Providers.FirstAsync(x => x.Code == providerIdentifier.Key, cancellationToken).ConfigureAwait(false),
                Provider_Release_Identifier = providerIdentifier.Value,
                Release = newRelease
            });
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

        Release_Album releaseAlbum = new()
        {
            Release = newRelease,
            Country = item.Country ?? string.Empty,
            Market = "FR",
            Label = item.Label ?? string.Empty,
            Tracklist_Albums = tracklist
        };

        foreach (var genreStr in item.Genres)
        {
            (bool Found, Genre_Album Genre) genreResult = await _genreAlbumRepository.TryFindGenreAsync(genreStr, cancellationToken).ConfigureAwait(false);

            if (genreResult.Found)
            {
                Genre_Album? genreInCurrentContext = await context.Genre_Albums.FindAsync([genreResult.Genre.Genre_Album_ID], cancellationToken).ConfigureAwait(false);
                if (genreInCurrentContext != null)
                {
                    releaseAlbum.Genre_Albums.Add(genreInCurrentContext);
                }
            }
            else
            {
                await _genreAlbumRepository.InsertAsync(genreStr, cancellationToken).ConfigureAwait(false);

                (bool Found, Genre_Album Genre) newGenreResult = await _genreAlbumRepository.TryFindGenreAsync(genreStr, cancellationToken).ConfigureAwait(false);
                if (newGenreResult.Found)
                {
                    Genre_Album? genreInCurrentContext = await context.Genre_Albums.FindAsync([genreResult.Genre.Genre_Album_ID], cancellationToken).ConfigureAwait(false);
                    if (genreInCurrentContext != null)
                    {
                        releaseAlbum.Genre_Albums.Add(genreInCurrentContext);
                    }
                }
            }
        }

        context.Release_Albums.Add(releaseAlbum);

        try
        {
            int rowAffected = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            result.IsSuccess = rowAffected >= 0;
            result.RowAffected = rowAffected;
            return result;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Release_Album), context?.Database?.GetDbConnection().ConnectionString, ex);
            return result;
        }
        catch (Exception ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Release_Album), context?.Database?.GetDbConnection().ConnectionString, ex);
            throw;
        }
    }

    /// <summary>
    /// Counts the total number of vinyl releases for a specific market.
    /// </summary>
    public async Task<int> CountAsync(string market, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        return await context.V_LastVinylReleases
                        .CountAsync(cancellationToken)
                        .ConfigureAwait(false);
    }

    /// <summary>
    /// Checks if an album already exists in the database based on provider identifiers.
    /// </summary>
    public async Task<bool> ExistsAsync(AlbumModel item, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        foreach (var providerItem in item.ProvidersIdentifier)
        {
            bool exists = await context.Provider_Releases
                                    .AnyAsync(x => x.Provider.Code == providerItem.Key
                                        && x.Provider_Release_Identifier == providerItem.Value,
                                        cancellationToken)
                                    .ConfigureAwait(false);

            if (exists)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Retrieves the most recently added vinyl release.
    /// </summary>
    public async Task<V_LastVinylRelease?> GetLastAsync(CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        return await context.V_LastVinylReleases
                        .OrderBy(x => x.Release_ID)
                        .LastOrDefaultAsync(cancellationToken)
                        .ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all vinyl releases.
    /// </summary>
    public async Task<IReadOnlyCollection<V_LastVinylRelease>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        var releases = await context.V_LastVinylReleases
                        .ToListAsync(cancellationToken)
                        .ConfigureAwait(false);

        return releases.AsReadOnly();
    }

    /// <summary>
    /// Retrieves vinyl releases for a specific market with pagination support.
    /// </summary>
    public async Task<IReadOnlyCollection<V_LastVinylRelease>> GetAllAsync(string market, int offset, int limit, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        List<V_LastVinylRelease> releases = await context.V_LastVinylReleases
                                                .OrderByDescending(x => x.Release_ID)
                                                .Skip(offset)
                                                .Take(limit)
                                                .ToListAsync(cancellationToken)
                                                .ConfigureAwait(false);

        return releases.AsReadOnly();
    }

    /// <summary>
    /// Retrieves a complete album release including tracks, genres, and assets by its identifier.
    /// </summary>
    public async Task<Release_Album?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        return await context.Release_Albums
                        .Include(x => x.Tracklist_Albums)
                        .ThenInclude(x => x.Track_Album)
                        .Include(x => x.Genre_Albums)
                        .Include(x => x.Release)
                        .ThenInclude(x => x.Asset_Releases)
                        .ThenInclude(x => x.Asset)
                        .Include(x => x.Release)
                        .ThenInclude(x => x.Author_Releases)
                        .ThenInclude(x => x.Author)
                        .FirstOrDefaultAsync(x => x.Release != null
                                        && x.Release.GUID == id.ToString(),
                                        cancellationToken)
                        .ConfigureAwait(false);
    }
}