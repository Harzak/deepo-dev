using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Result;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.DAL.Repository.Interfaces;

/// <summary>
/// Repository interface for managing album genre data including normalization and search operations.
/// </summary>
public interface IGenreAlbumRepository
{
    /// <summary>
    /// Retrieves all album genres from the database.
    /// </summary>
    Task<ReadOnlyCollection<Genre_Album>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if a specific genre already exists in the database.
    /// </summary>
    Task<bool> ExistsAsync(Genre_Album genre, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Inserts a new genre into the database with normalized name formatting.
    /// </summary>
    Task<DatabaseOperationResult> InsertAsync(string genreName, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Attempts to find an existing genre by performing normalized name matching.
    /// </summary>
    Task<(bool Found, Genre_Album Genre)> TryFindGenreAsync(string genreSearched, CancellationToken cancellationToken = default);
}