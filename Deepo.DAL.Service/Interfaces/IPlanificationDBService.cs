using Deepo.DAL.EF.Models;
using Framework.Common.Worker.Interfaces;
using System.Collections.ObjectModel;

namespace Deepo.DAL.Service.Feature.Fetcher;
public interface IPlanificationDBService
{
    ReadOnlyCollection<V_FetcherPlannification>? GetAll();

    bool Delete(IWorker worker);

    bool AddOneShot(IWorker worker);

    bool AddHourly(IWorker worker, int startMinute);

    bool AddDaily(IWorker worker, int startHour, int startMinute);

    bool UpdateDateNextStart(Guid fetcherGUID, DateTime dateNextStart);
}
