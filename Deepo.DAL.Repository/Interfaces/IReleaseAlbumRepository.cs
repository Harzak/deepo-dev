using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.DAL.Repository.Interfaces;

/// <summary>
/// Repository interface for managing album release data including insertion, existence checks, and retrieval operations.
/// </summary>
public interface IReleaseAlbumRepository
{
    /// <summary>
    /// Inserts a complete album with associated artists, genres, and tracks into the database.
    /// </summary>
    Task<DatabaseOperationResult> InsertAsync(AlbumModel item, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Counts the total number of vinyl releases for a specific market.
    /// </summary>
    Task<int> CountAsync(string market, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if an album already exists in the database based on provider identifiers.
    /// </summary>
    Task<bool> ExistsAsync(AlbumModel item, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves the most recently added vinyl release.
    /// </summary>
    Task<V_LastVinylRelease?> GetLastAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves all vinyl releases.
    /// </summary>
    Task<IReadOnlyCollection<V_LastVinylRelease>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves vinyl releases for a specific market with pagination support.
    /// </summary>
    Task<IReadOnlyCollection<V_LastVinylRelease>> GetAllAsync(string market, int offset, int limit, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a complete album release including tracks, genres, and assets by its identifier.
    /// </summary>
    Task<Release_Album?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}