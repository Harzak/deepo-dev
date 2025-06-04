using Deepo.DAL.Service.Feature.Fetcher;
using Deepo.Fetcher.WPF.Features.FetcherGrid.Provider;
using Framework.WPF.Behavior.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.Fetcher.WPF.ViewModels
{
    internal sealed class FetcherListViewModel : ViewModelBase
    {
        private readonly IFetcherDBService _fetcherDBService;
        private readonly IFetcherGridProviderFactory _fetcherGridProviderFactory;

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
                    SelectedFetcherViewModel = new SelectedFetcherViewModel(_selectedFetcher,
                        _fetcherDBService,
                        _fetcherGridProviderFactory.CreateFetcherGridProvider(value.Fetcher_GUID));
                    OnPropertyChanged(nameof(SelectedFetcherViewModel));
                }
            }
        }


        internal FetcherListViewModel(IFetcherDBService fetcherDBService, IFetcherGridProviderFactory fetcherGridProviderFactory)
        {
            _fetcherDBService = fetcherDBService;
            _fetcherGridProviderFactory = fetcherGridProviderFactory;

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
}
