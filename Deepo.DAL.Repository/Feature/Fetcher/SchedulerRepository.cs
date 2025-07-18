using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.DAL.Repository.LogMessage;
using Deepo.Framework.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Feature.Fetcher;

public sealed class SchedulerRepository : ISchedulerRepository
{
    private readonly ILogger<SchedulerRepository> _logger;
    private readonly IDbContextFactory<DEEPOContext> _contextFactory;

    public SchedulerRepository(IDbContextFactory<DEEPOContext> contextFactory, ILogger<SchedulerRepository> logger)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }


    public async Task<string?> GetCronExpressionForFectherAsync(string fectherIdentifier, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        Scheduler? scheduler = await context.Schedulers
                                            .Include(x => x.Schedule)
                                            .Include(x => x.Fetcher)
                                            .FirstOrDefaultAsync(s => s.Fetcher.Fetcher_GUID == fectherIdentifier, cancellationToken)                                  
                                            .ConfigureAwait(false);
        if (scheduler?.Schedule != null)
        {
            return scheduler.Schedule.CronExpression;
        }
        return null;
    }

    public async Task<Dictionary<string, string>> GetAllFetcherCronExpressionAsync(CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        return await context.Schedulers
                            .Include(x => x.Schedule)
                            .Include(x => x.Fetcher)
                            .Where(x => x.Fetcher != null && x.Schedule != null && x.Schedule.CronExpression != null)
                            .ToDictionaryAsync(
                                keySelector: x => x.Fetcher.Fetcher_GUID,
                                elementSelector: x => x.Schedule?.CronExpression!,
                                cancellationToken: cancellationToken
                            )
                            .ConfigureAwait(false);
    }
}