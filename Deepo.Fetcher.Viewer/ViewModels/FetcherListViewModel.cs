using Deepo.DAL.Service.Feature.Fetcher;
using Deepo.Fetcher.Viewer.Interfaces;
using Framework.WPF.Behavior.ViewModel;
using System.Collections.Generic;
using System.Linq;
using EF = Deepo.DAL.EF.Models;

namespace Deepo.Fetcher.Viewer.ViewModels;

internal sealed class FetcherListViewModel : ViewModelBase
{
    private readonly IFetcherDBService _fetcherDBService;
    private readonly IFetcherListenerFactory _fetcherListenerFactory;

    public SelectedFetcherViewModel? SelectedFetcherViewModel { get; private set; }

    public IEnumerable<EF.Fetcher> Fetchers { get; }

    private EF.Fetcher? _selectedFetcher;
    public EF.Fetcher? SelectedFetcher
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

        this.Fetchers = fetchers != null ? fetchers : new List<EF.Fetcher>();

        if (Fetchers.Any())
        {
            SelectedFetcher = Fetchers.First();
            OnPropertyChanged(nameof(Fetchers));
            OnPropertyChanged(nameof(SelectedFetcher));
        }
    }

}
