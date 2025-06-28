using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.DAL.Repository.LogMessage;
using Framework.Common.Utils.Time.Provider;
using Framework.Common.Worker.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Feature.Fetcher;

public class PlanificationRepository : IPlanificationRepository
{
    private readonly ILogger<PlanificationRepository> _logger;
    private readonly DEEPOContext _dbContext;
    private readonly ITimeProvider _timeProvier;

    private const string DAILY_PLANNING_CODE = "DAILY";
    private const string HOURLY_PLANNING_CODE = "HOURLY";
    private const string ONESHOT_PLANNING_CODE = "ONESHOT";

    public PlanificationRepository(DEEPOContext dbContext, ITimeProvider datetimeprovider, ILogger<PlanificationRepository> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
        _timeProvier = datetimeprovider;
    }

    public bool Delete(IWorker worker)
    {
        if (!_dbContext.Fetchers.Any(x => x.Fetcher_GUID == worker.ID.ToString()))
        {
            return true;
        }
        Planification planification = _dbContext.Planifications.First(x => x.Fetcher.Fetcher_GUID == worker.ID.ToString());
        _dbContext.Planifications.Remove(planification);
        try
        {
            return _dbContext.SaveChanges() > 0;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToRemove(_logger, nameof(Planification), _dbContext?.Database?.GetDbConnection().ConnectionString, ex);
            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool AddOneShot(IWorker worker)
    {
        if (worker != null)
        {
            return Add(worker, ONESHOT_PLANNING_CODE);
        }
        return false;
    }

    public bool AddHourly(IWorker worker, int startMinute)
    {
        if (worker != null)
        {
            return Add(worker, HOURLY_PLANNING_CODE, null, startMinute);
        }
        return false;
    }

    public bool AddDaily(IWorker worker, int startHour, int startMinute)
    {
        if (worker != null)
        {
            return Add(worker, DAILY_PLANNING_CODE, startHour, startMinute);
        }
        return false;
    }

    public bool UpdateDateNextStart(Guid fetcherGUID, DateTime dateNextStart)
    {
        if (!_dbContext.Planifications.Any(x => x.Fetcher.Fetcher_GUID == fetcherGUID.ToString()))
        {
            return false;
        }
        _dbContext.Planifications.First(x => x.Fetcher.Fetcher_GUID == fetcherGUID.ToString()).DateNextStart =
            new DateTime(dateNextStart.Year, dateNextStart.Month, dateNextStart.Day, dateNextStart.Hour, dateNextStart.Minute, dateNextStart.Second);
        try
        {
            return _dbContext.SaveChanges() > 0;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToUpdate(_logger, nameof(Planification), _dbContext?.Database?.GetDbConnection().ConnectionString, ex);
            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private bool Add(IWorker worker, string codePlanning, int? startHour = null, int? startMinute = null)
    {
        if (!_dbContext.Fetchers.Any(x => x.Fetcher_GUID == worker.ID.ToString()))
        {
            return false;
        }

        _dbContext.Planifications.Add(new Planification
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
            PlanificationType = _dbContext.PlanificationTypes.First(x => x.Code == codePlanning)
        });

        try
        {
            return _dbContext.SaveChanges() == 1;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Planification), _dbContext?.Database?.GetDbConnection().ConnectionString, ex);
            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public ReadOnlyCollection<V_FetcherPlannification>? GetAll()
    {
        return _dbContext.V_FetcherPlannifications.ToList().AsReadOnly();
    }
}
