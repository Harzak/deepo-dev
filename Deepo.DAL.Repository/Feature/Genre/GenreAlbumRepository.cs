using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.DAL.Repository.LogMessage;
using Deepo.DAL.Repository.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.DAL.Repository.Feature.Genre;

/// <summary>
/// Repository for managing album genre data including normalization and search operations.
/// </summary>
public class GenreAlbumRepository : IGenreAlbumRepository
{
    private static char[] ALLOWED_GENRE_NAME_CHAR = ['-', '/', ' '];

    private readonly ILogger _logger;
    private readonly IDbContextFactory<DEEPOContext> _contextFactory;

    public GenreAlbumRepository(IDbContextFactory<DEEPOContext> contextFactory, ILogger<GenreAlbumRepository> logger)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    /// <summary>
    /// Retrieves all album genres from the database.
    /// </summary>
    public async Task<ReadOnlyCollection<Genre_Album>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        List<Genre_Album> genres = await context.Genre_Albums
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);
            
        return genres.AsReadOnly();
    }

    /// <summary>
    /// Checks if a specific genre already exists in the database.
    /// </summary>
    public async Task<bool> ExistsAsync(Genre_Album genre, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        return await context.Genre_Albums
                        .AnyAsync(x => x.Identifier == genre.Identifier, cancellationToken)
                        .ConfigureAwait(false);
    }

    /// <summary>
    /// Inserts a new genre into the database with normalized name formatting.
    /// </summary>
    public async Task<DatabaseOperationResult> InsertAsync(string genreName, CancellationToken cancellationToken = default)
    {
        DatabaseOperationResult result = new();

        if (string.IsNullOrEmpty(genreName))
        {
            return result;
        }

        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        string normalizedGenreName = NormalizeGenreName(genreName);

        context.Genre_Albums.Add(new Genre_Album()
        {
            Identifier = Guid.NewGuid().ToString(),
            Name = PretifyGenreName(normalizedGenreName)
        });

        try
        {
            int rowAffected = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            result.IsSuccess = rowAffected >= 0;
            result.RowAffected = rowAffected;
            return result;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Release_Album), context?.Database?.GetDbConnection()?.ConnectionString, ex);
            return result;
        }
        catch (Exception ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Release_Album), context?.Database?.GetDbConnection()?.ConnectionString, ex);
            throw;
        }
    }

    /// <summary>
    /// Attempts to find an existing genre by performing normalized name matching.
    /// </summary>
    public async Task<(bool Found, Genre_Album Genre)> TryFindGenreAsync(string genreSearched, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(genreSearched))
        {
            return (false, new Genre_Album());
        }

        string normalizedInput = NormalizeGenreName(genreSearched);
        var allGenres = await GetAllAsync(cancellationToken).ConfigureAwait(false);
        
        foreach (Genre_Album genreStored in allGenres)
        {
            string normalizedGenreName = NormalizeGenreName(genreStored.Name);

            if (normalizedGenreName == normalizedInput)
            {
                return (true, genreStored);
            }
        }
        
        return (false, new Genre_Album());
    }

    private string NormalizeGenreName(string genreName)
    {
        string normalized = genreName.ToUpperInvariant();

        List<char> resultChars = [];

        foreach (char c in normalized)
        {
            if (char.IsLetter(c) || ALLOWED_GENRE_NAME_CHAR.Contains(c))
            {
                resultChars.Add(c);
            }
            else
            {
                resultChars.Add(' ');
            }
        }

        normalized = new string([.. resultChars]);

        while (normalized.Contains("  ", StringComparison.InvariantCulture))
        {
            normalized = normalized.Replace("  ", " ", StringComparison.InvariantCulture);
        }

        return normalized.Trim();
    }

    private string PretifyGenreName(string genreName)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(genreName.ToLowerInvariant());
    }
}