using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Viewer.Interfaces;
using Framework.WPF.Behavior.ViewModel;
using System.Collections.Generic;
using System.Linq;
using EF = Deepo.DAL.EF.Models;

namespace Deepo.Fetcher.Viewer.ViewModels;

internal sealed class FetcherListViewModel : ViewModelBase
{
    private readonly IFetcherRepository _fetcherRepository;
    private readonly IFetcherListener _fetcherListener;

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
                SelectedFetcherViewModel = new SelectedFetcherViewModel(_selectedFetcher,_fetcherRepository, _fetcherListener);
                OnPropertyChanged(nameof(SelectedFetcherViewModel));
            }
        }
    }


    internal FetcherListViewModel(IFetcherRepository fetcherRepository, IFetcherListener fetcherListener)
    {
        _fetcherRepository = fetcherRepository;
        _fetcherListener = fetcherListener;

        var fetchers = _fetcherRepository.GetAllAsync().Result;

        this.Fetchers = fetchers != null ? fetchers : new List<EF.Fetcher>();

        if (Fetchers.Any())
        {
            SelectedFetcher = Fetchers.First();
            OnPropertyChanged(nameof(Fetchers));
            OnPropertyChanged(nameof(SelectedFetcher));
        }
    }

}
