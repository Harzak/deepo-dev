using Deepo.DAL.EF.Models;
using Deepo.Framework.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.DAL.Repository.Interfaces;

public interface IPlanificationRepository
{
    Task<bool> DeleteAsync(IWorker worker, CancellationToken cancellationToken = default);
    Task<bool> AddOneShotAsync(IWorker worker, CancellationToken cancellationToken = default);
    Task<bool> AddHourlyAsync(IWorker worker, int startMinute, CancellationToken cancellationToken = default);
    Task<bool> AddDailyAsync(IWorker worker, int startHour, int startMinute, CancellationToken cancellationToken = default);
    Task<bool> UpdateDateNextStartAsync(Guid fetcherGUID, DateTime dateNextStart, CancellationToken cancellationToken = default);
    Task<ReadOnlyCollection<V_FetcherPlannification>> GetAllAsync(CancellationToken cancellationToken = default);
}