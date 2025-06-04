using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.LogMessage;
using Framework.Common.Utils.Time.Provider;
using Framework.Common.Worker.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Deepo.DAL.Service.Feature.Fetcher;

public class FetcherExecutionDBService : IFetcherExecutionDBService
{
    private readonly ILogger<FetcherExecutionDBService> _logger;
    private readonly DEEPOContext _dbContext;
    private readonly ITimeProvider _timeProvier;

    public FetcherExecutionDBService(DEEPOContext dbContext, ITimeProvider datetimeprovider, ILogger<FetcherExecutionDBService> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
        _timeProvier = datetimeprovider;
    }

    public bool LogStart(IWorker worker)
    {
        if (!_dbContext.Fetchers.Any(x => x.Fetcher_GUID == worker.ID.ToString()))
        {
            return false;
        }

        _dbContext.Executions.Add(new Execution()
        {
            Fetcher = _dbContext.Fetchers.First(x => x.Fetcher_GUID == worker.ID.ToString()),
            StartedAt = _timeProvier.DateTimeUTCNow()
        });

        try
        {
            return _dbContext.SaveChanges() == 1;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Execution), _dbContext?.Database?.GetDbConnection().ConnectionString, ex);
            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool LogEnd(IWorker worker)
    {
        if (!_dbContext.Fetchers.Any(x => x.Fetcher_GUID == worker.ID.ToString()))
        {
            return false;
        }

        var execution = _dbContext.Executions.FirstOrDefault(x => x.Fetcher == _dbContext.Fetchers.First(x => x.Fetcher_GUID == worker.ID.ToString()));
        if (execution is null)
        {
            return false;
        }

        execution.EndedAt = _timeProvier.DateTimeUTCNow();

        try
        {
            return _dbContext.SaveChanges() == 1;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Execution), _dbContext?.Database?.GetDbConnection().ConnectionString, ex);
            return false;
        }
        catch (Exception ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Execution), _dbContext?.Database?.GetDbConnection().ConnectionString, ex);
            throw;
        }
    }
    public IEnumerable<V_FetchersLastExecution> GetFetcherExecutions()
    {
        return _dbContext.V_FetchersLastExecutions.ToList();
    }

}
