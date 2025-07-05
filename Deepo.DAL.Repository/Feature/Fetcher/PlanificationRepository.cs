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

public sealed class PlanificationRepository : IPlanificationRepository
{
    private readonly ILogger<PlanificationRepository> _logger;
    private readonly IDbContextFactory<DEEPOContext> _contextFactory;

    private const string DAILY_PLANNING_CODE = "DAILY";
    private const string HOURLY_PLANNING_CODE = "HOURLY";
    private const string ONESHOT_PLANNING_CODE = "ONESHOT";

    public PlanificationRepository(IDbContextFactory<DEEPOContext> contextFactory, ILogger<PlanificationRepository> logger)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<bool> DeleteAsync(IWorker worker, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        if (!await context.Fetchers
                    .AnyAsync(x => x.Fetcher_GUID == worker.ID.ToString(), cancellationToken)
                    .ConfigureAwait(false))
        {
            return true;
        }

        Planification planification = await context.Planifications
                                        .FirstAsync(x => x.Fetcher.Fetcher_GUID == worker.ID.ToString(), cancellationToken)
                                        .ConfigureAwait(false);

        context.Planifications.Remove(planification);

        try
        {
            return await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToRemove(_logger, nameof(Planification), context?.Database?.GetDbConnection().ConnectionString, ex);
            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> AddOneShotAsync(IWorker worker, CancellationToken cancellationToken = default)
    {
        if (worker != null)
        {
            return await AddAsync(worker, ONESHOT_PLANNING_CODE, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        return false;
    }

    public async Task<bool> AddHourlyAsync(IWorker worker, int startMinute, CancellationToken cancellationToken = default)
    {
        if (worker != null)
        {
            return await AddAsync(worker, HOURLY_PLANNING_CODE, null, startMinute, cancellationToken).ConfigureAwait(false);
        }
        return false;
    }

    public async Task<bool> AddDailyAsync(IWorker worker, int startHour, int startMinute, CancellationToken cancellationToken = default)
    {
        if (worker != null)
        {
            return await AddAsync(worker, DAILY_PLANNING_CODE, startHour, startMinute, cancellationToken).ConfigureAwait(false);
        }
        return false;
    }

    public async Task<bool> UpdateDateNextStartAsync(Guid fetcherGUID, DateTime dateNextStart, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        if (!await context.Planifications
                    .AnyAsync(x => x.Fetcher.Fetcher_GUID == fetcherGUID.ToString(), cancellationToken)
                    .ConfigureAwait(false))
        {
            return false;
        }

        Planification planification = await context.Planifications
                            .FirstAsync(x => x.Fetcher.Fetcher_GUID == fetcherGUID.ToString(), cancellationToken)
                            .ConfigureAwait(false);

        planification.DateNextStart = new DateTime(
            dateNextStart.Year,
            dateNextStart.Month,
            dateNextStart.Day,
            dateNextStart.Hour,
            dateNextStart.Minute,
            dateNextStart.Second);

        try
        {
            return await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToUpdate(_logger, nameof(Planification), context?.Database?.GetDbConnection().ConnectionString, ex);
            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<bool> AddAsync(IWorker worker, string codePlanning, int? startHour = null, int? startMinute = null, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        if (!await context.Fetchers
                            .AnyAsync(x => x.Fetcher_GUID == worker.ID.ToString(), cancellationToken)
                            .ConfigureAwait(false))
        {
            return false;
        }

        var planificationType = await context.PlanificationTypes
                                        .FirstAsync(x => x.Code == codePlanning, cancellationToken)
                                        .ConfigureAwait(false);

        context.Planifications.Add(new Planification
        {
            Fetcher = new Models.Fetcher
            {
                Fetcher_GUID = worker.ID.ToString(),
                Name = worker.Name
            },
            Planning = new Planning()
            {
                HourStart = startHour,
                MinuteStart = startMinute
            },
            PlanificationType = planificationType
        });

        try
        {
            return await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false) == 1;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Planification), context?.Database?.GetDbConnection().ConnectionString, ex);
            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<ReadOnlyCollection<V_FetcherPlannification>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        List<V_FetcherPlannification> planifications = await context.V_FetcherPlannifications
                                                        .ToListAsync(cancellationToken)
                                                        .ConfigureAwait(false);

        return planifications.AsReadOnly();
    }
}
