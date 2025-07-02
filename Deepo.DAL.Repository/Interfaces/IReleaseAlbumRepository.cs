using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.DAL.Repository.Interfaces;

public interface IReleaseAlbumRepository
{
    // Async methods
    Task<DatabaseOperationResult> InsertAsync(AlbumModel item, CancellationToken cancellationToken = default);
    Task<int> CountAsync(string market, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(AlbumModel item, CancellationToken cancellationToken = default);
    Task<V_LastVinylRelease?> GetLastAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<V_LastVinylRelease>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<V_LastVinylRelease>> GetAllAsync(string market, int offset, int limit, CancellationToken cancellationToken = default);
    Task<Release_Album?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}