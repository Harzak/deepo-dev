using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Deepo.DAL.Repository.Feature.Fetcher;

public sealed class FetcherHttpRequestRepository : IFetcherHttpRequestRepository
{
    private readonly IDbContextFactory<DEEPOContext> _contextFactory;

    public FetcherHttpRequestRepository(IDbContextFactory<DEEPOContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<HttpRequest?> GetLastAsync(CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        return await context.HttpRequests
                        .OrderByDescending(x => x.HttpRequest_ID)
                        .FirstOrDefaultAsync( cancellationToken)
                        .ConfigureAwait(false);
    }
}