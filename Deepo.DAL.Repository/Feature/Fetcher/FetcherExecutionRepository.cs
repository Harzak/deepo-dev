using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.DAL.Repository.LogMessage;
using Deepo.Framework.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Deepo.DAL.Repository.Feature.Fetcher;

public sealed class FetcherExecutionRepository : IFetcherExecutionRepository
{
    private readonly ILogger<FetcherExecutionRepository> _logger;
    private readonly IDbContextFactory<DEEPOContext> _contextFactory;
    private readonly ITimeProvider _timeProvier;

    public FetcherExecutionRepository(IDbContextFactory<DEEPOContext> contextFactory, ITimeProvider datetimeprovider, ILogger<FetcherExecutionRepository> logger)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _timeProvier = datetimeprovider;
    }

    public async Task<bool> LogStartAsync(IWorker worker, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        bool fetcherExists = await context.Fetchers
                                    .AnyAsync(x => x.Fetcher_GUID == worker.ID.ToString(), cancellationToken)
                                    .ConfigureAwait(false);

        if (!fetcherExists)
        {
            return false;
        }

        var fetcher = await context.Fetchers
                                .FirstAsync(x => x.Fetcher_GUID == worker.ID.ToString(), cancellationToken)
                                .ConfigureAwait(false);

        context.Executions.Add(new Execution()
        {
            Fetcher = fetcher,
            StartedAt = _timeProvier.DateTimeUTCNow()
        });

        try
        {
            return await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false) == 1;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Execution), context?.Database?.GetDbConnection().ConnectionString, ex);
            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> LogEndAsync(IWorker worker, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        bool fetcherExists = await context.Fetchers
                                    .AnyAsync(x => x.Fetcher_GUID == worker.ID.ToString(), cancellationToken)
                                    .ConfigureAwait(false);

        if (!fetcherExists)
        {
            return false;
        }

        var fetcher = await context.Fetchers
                            .FirstAsync(x => x.Fetcher_GUID == worker.ID.ToString(), cancellationToken)
                            .ConfigureAwait(false);

        var execution = await context.Executions
                                .FirstOrDefaultAsync(x => x.Fetcher == fetcher, cancellationToken)
                                .ConfigureAwait(false);

        if (execution is null)
        {
            return false;
        }

        execution.EndedAt = _timeProvier.DateTimeUTCNow();

        try
        {
            return await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false) == 1;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Execution), context?.Database?.GetDbConnection().ConnectionString, ex);
            return false;
        }
        catch (Exception ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Execution), context?.Database?.GetDbConnection().ConnectionString, ex);
            throw;
        }
    }

    public async Task<IEnumerable<V_FetchersLastExecution>> GetFetcherExecutionsAsync(CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        return await context.V_FetchersLastExecutions
                        .ToListAsync(cancellationToken)
                        .ConfigureAwait(false);
    }

    public async Task<V_FetchersLastExecution?> GetLastFetcherExecutionAsync(CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        return await context.V_FetchersLastExecutions
                        .OrderByDescending(x => x.StartedAt)
                        .FirstOrDefaultAsync(cancellationToken)
                        .ConfigureAwait(false);
    }
}