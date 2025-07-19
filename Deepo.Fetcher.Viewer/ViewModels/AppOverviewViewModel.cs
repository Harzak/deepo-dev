using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Feature.Fetcher;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Viewer.WPF;
using Deepo.Fetcher.Viewer.Interfaces;
using Deepo.Fetcher.Viewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Viewer.ViewModels;

/// <summary>
/// Provides an overview of fetcher operations, including active and inactive fetcher counts with real-time updates.
/// </summary>
public class AppOverviewViewModel : BaseViewModel
{
    private readonly IFetcherExecutionRepository _fetcherExecutionRepository;
    private readonly IFetcherListener _fetcherListener;
    private IEnumerable<V_FetchersLastExecution> _fetcherExecutions;

    /// <summary>
    /// Gets the count of currently active fetchers that are running.
    /// </summary>
    public int ActiveFetcherCount
    {
        get
        {
            return _fetcherExecutions.Where(x => x.StartedAt != null && x.StartedAt > x.EndedAt).Count();
        }
    }

    /// <summary>
    /// Gets the count of inactive fetchers that are not currently running.
    /// </summary>
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

    /// <summary>
    /// Asynchronously initializes the view model by loading fetcher execution data.
    /// </summary>
    public async Task InitializeAsync()
    {
        await RefreshFetcherExecutions().ConfigureAwait(false);
    }

    private async void OnFetcherExecutionRowAdded(object? sender, FetcherExecutionEventArgs e)
    {
        await App.Current.Dispatcher.BeginInvoke(async () =>
        {
            await RefreshFetcherExecutions().ConfigureAwait(false);
        });
    }

    private async Task RefreshFetcherExecutions()
    {
        _fetcherExecutions = await _fetcherExecutionRepository.GetFetcherExecutionsAsync().ConfigureAwait(false);
        OnPropertyChanged(nameof(ActiveFetcherCount));
        OnPropertyChanged(nameof(InactiveFetcherCount));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_fetcherListener != null)
            {
                _fetcherListener.FetcherExecutionRowAdded -= OnFetcherExecutionRowAdded;
            }
        }
        base.Dispose(disposing);
    }
}
