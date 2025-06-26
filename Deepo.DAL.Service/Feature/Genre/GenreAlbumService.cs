using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Interfaces;
using Deepo.DAL.Service.LogMessage;
using Deepo.DAL.Service.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;

namespace Deepo.DAL.Service.Feature.Genre;

public class GenreAlbumService : IGenreAlbumService
{
    private static char[] ALLOWED_GENRE_NAME_CHAR = ['-', '/', ' '];

    private readonly ILogger _logger;
    private readonly DEEPOContext _dbContext;

    public GenreAlbumService(DEEPOContext dbContext, ILogger<GenreAlbumService> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public ReadOnlyCollection<Genre_Album> GetAll()
    {
        return _dbContext.Genre_Albums.ToList().AsReadOnly();
    }

    public bool Exists(Genre_Album genre)
    {
        return _dbContext.Genre_Albums.Any(x => x.Identifier == genre.Identifier);
    }

    public async Task<DatabaseOperationResult> Insert(string genreName, CancellationToken cancellationToken)
    {
        DatabaseOperationResult result = new();

        if (string.IsNullOrEmpty(genreName))
        {
            return result;
        }

        string normalizedGenreName = NormalizeGenreName(genreName);

        _dbContext.Genre_Albums.Add(new Genre_Album()
        {
            Identifier = Guid.NewGuid().ToString(),
            Name = PretifyGenreName(normalizedGenreName)
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

    public bool TryFindGenre(string genreSearched, out Genre_Album genreFind)
    {
        genreFind = new Genre_Album();
        if (string.IsNullOrWhiteSpace(genreSearched))
        {
            return false;
        }

        string normalizedInput = NormalizeGenreName(genreSearched);
        foreach (Genre_Album genreStored in this.GetAll())
        {
            string normalizedGenreName = NormalizeGenreName(genreStored.Name);

            if (normalizedGenreName == normalizedInput)
            {
                genreFind = genreStored;
                return true;
            }
        }
        return false;
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