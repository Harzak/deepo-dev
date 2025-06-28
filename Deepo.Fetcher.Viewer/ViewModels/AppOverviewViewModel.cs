using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Feature.Fetcher;
using Framework.WPF.Behavior.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Deepo.Fetcher.Viewer.ViewModels;

public class AppOverviewViewModel : ViewModelBase
{
    private readonly IFetcherExecutionDBService _fetcherExecutionDBService;

    private readonly List<V_FetchersLastExecution> _fetcherExecutions;

    public AppOverviewViewModel(IFetcherExecutionDBService fetcherExecutionDBService)
    {
        _fetcherExecutionDBService = fetcherExecutionDBService;
        _fetcherExecutions = _fetcherExecutionDBService.GetFetcherExecutions().ToList();
    }

    public int ActiveFetcherCount
    {
        get
        {
            return _fetcherExecutions.Where(x => x.StartedAt != null && x.EndedAt is null).Count();
        }
    }

    public int InactiveFetcherCount
    {
        get
        {
            return _fetcherExecutions.Where(x => x.StartedAt is null || x.EndedAt != null).Count();
        }
    }
}
