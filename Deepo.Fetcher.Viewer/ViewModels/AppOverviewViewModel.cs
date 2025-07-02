using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Feature.Fetcher;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Viewer.Interfaces;
using Deepo.Fetcher.Viewer.Models;
using Framework.WPF.Behavior.ViewModel;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Viewer.ViewModels;

public class AppOverviewViewModel : ViewModelBase
{
    private readonly IFetcherExecutionRepository _fetcherExecutionRepository;
    private readonly IFetcherListener _fetcherListener;
    private IEnumerable<V_FetchersLastExecution> _fetcherExecutions;

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

    public AppOverviewViewModel(
        IFetcherExecutionRepository fetcherExecutionRepository, 
        IFetcherListener fetcherListener)
    {
        _fetcherExecutionRepository = fetcherExecutionRepository;
        _fetcherListener = fetcherListener;

        _fetcherExecutions = [];
        _fetcherListener.FetcherExecutionRowAdded += OnFetcherExecutionRowAdded;
    }

    public async Task InitializeAsync()
    {
        _fetcherExecutions = await _fetcherExecutionRepository.GetFetcherExecutionsAsync().ConfigureAwait(false);
    }

    private async void OnFetcherExecutionRowAdded(object? sender, FetcherExecutionEventArgs e)
    {
        _fetcherExecutions = await _fetcherExecutionRepository.GetFetcherExecutionsAsync().ConfigureAwait(false);
    }
}
