using Deepo.DAL.Service.Feature.Fetcher;
using Deepo.Fetcher.Viewer.Interfaces;
using Framework.WPF.Behavior.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.Fetcher.Viewer.ViewModels;

internal sealed class FetcherListViewModel : ViewModelBase
{
    private readonly IFetcherDBService _fetcherDBService;
    private readonly IFetcherListenerFactory _fetcherListenerFactory;

    public SelectedFetcherViewModel? SelectedFetcherViewModel { get; private set; }

    public IEnumerable<Models.Fetcher> Fetchers { get; }

    private Models.Fetcher? _selectedFetcher;
    public Models.Fetcher? SelectedFetcher
    {
        get => _selectedFetcher;
        set
        {
            if (value != null && value != _selectedFetcher)
            {
                _selectedFetcher = value;
                SelectedFetcherViewModel?.Dispose();
                SelectedFetcherViewModel = new SelectedFetcherViewModel(_selectedFetcher,_fetcherDBService, _fetcherListenerFactory.CreateFetcherListener());
                OnPropertyChanged(nameof(SelectedFetcherViewModel));
            }
        }
    }


    internal FetcherListViewModel(IFetcherDBService fetcherDBService,
        IFetcherListenerFactory fetcherListenerFactory)
    {
        _fetcherDBService = fetcherDBService;
        _fetcherListenerFactory = fetcherListenerFactory;

        var fetchers = _fetcherDBService.GetAll();

        this.Fetchers = fetchers != null ? fetchers : new List<Models.Fetcher>();

        if (Fetchers.Any())
        {
            SelectedFetcher = Fetchers.First();
            OnPropertyChanged(nameof(Fetchers));
            OnPropertyChanged(nameof(SelectedFetcher));
        }
    }

}
