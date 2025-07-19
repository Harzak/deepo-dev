using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Viewer.Interfaces;
using System.Collections.Generic;
using System.Linq;
using EF = Deepo.DAL.EF.Models;

namespace Deepo.Fetcher.Viewer.ViewModels;

/// <summary>
/// Manages the display and selection of fetchers in a list interface with detailed fetcher information.
/// </summary>
internal sealed class FetcherListViewModel : BaseViewModel
{
    private readonly IFetcherRepository _fetcherRepository;
    private readonly IFetcherListener _fetcherListener;
    private EF.Fetcher? _selectedFetcher;

    /// <summary>
    /// Gets the view model for the currently selected fetcher with detailed information.
    /// </summary>
    public SelectedFetcherViewModel? SelectedFetcherViewModel { get; private set; }

    /// <summary>
    /// Gets the collection of all available fetchers.
    /// </summary>
    public IEnumerable<EF.Fetcher> Fetchers { get; }

    /// <summary>
    /// Gets or sets the currently selected fetcher from the list.
    /// </summary>
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

        this.Fetchers = fetchers != null ? fetchers : [];

        if (Fetchers.Any())
        {
            SelectedFetcher = Fetchers.First();
            OnPropertyChanged(nameof(Fetchers));
            OnPropertyChanged(nameof(SelectedFetcher));
        }
    }

}
