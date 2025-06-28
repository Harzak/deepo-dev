using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Feature.Fetcher;
using Deepo.DAL.Repository.Interfaces;
using Framework.WPF.Behavior.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Deepo.Fetcher.Viewer.ViewModels;

public class AppOverviewViewModel : ViewModelBase
{
    private readonly IFetcherExecutionRepository _fetcherExecutionRepository;

    private readonly List<V_FetchersLastExecution> _fetcherExecutions;

    public AppOverviewViewModel(IFetcherExecutionRepository fetcherExecutionRepository)
    {
        _fetcherExecutionRepository = fetcherExecutionRepository;
        _fetcherExecutions = _fetcherExecutionRepository.GetFetcherExecutions().ToList();
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
