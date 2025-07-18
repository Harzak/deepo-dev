using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Feature.Fetcher;

/// <summary>
/// Repository for managing fetcher configuration data and metadata.
/// </summary>
public sealed class FetcherRepository : IFetcherRepository
{
    private readonly IDbContextFactory<DEEPOContext> _contextFactory;

    public FetcherRepository(IDbContextFactory<DEEPOContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <summary>
    /// Retrieves all fetcher configurations.
    /// </summary>
    public async Task<ReadOnlyCollection<Models.Fetcher>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        List<Models.Fetcher> fetchers = await context.Fetchers
                                                .ToListAsync(cancellationToken)
                                                .ConfigureAwait(false);
            
        return fetchers.AsReadOnly();
    }

    /// <summary>
    /// Retrieves all fetchers with extended information.
    /// </summary>
    public async Task<ReadOnlyCollection<V_FetcherExtended>> GetAllExtendedAsync(CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        List<V_FetcherExtended> extended = await context.V_FetcherExtendeds
                                                    .ToListAsync(cancellationToken)
                                                    .ConfigureAwait(false);
            
        return extended.AsReadOnly();
    }

    /// <summary>
    /// Retrieves a fetcher by its name.
    /// </summary>
    public async Task<Models.Fetcher?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        return await context.Fetchers
                        .FirstOrDefaultAsync(x => x.Name == name, cancellationToken)
                        .ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a fetcher by its unique identifier.
    /// </summary>
    public async Task<Models.Fetcher?> GetByIdAsync(string identifier, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        return await context.Fetchers
                        .FirstOrDefaultAsync(x => x.Fetcher_GUID == identifier, cancellationToken)
                        .ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves extended fetcher information by identifier.
    /// </summary>
    public async Task<V_FetcherExtended?> GetExtendedAsync(string id, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        return await context.V_FetcherExtendeds
                        .Where(x => x.Fetcher_GUID == id)
                        .FirstOrDefaultAsync(cancellationToken)
                        .ConfigureAwait(false);
    }
}