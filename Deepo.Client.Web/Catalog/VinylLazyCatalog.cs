using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Dto;
using Deepo.Client.Web.EventBus.Vinyl;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Newtonsoft.Json;
using System.Globalization;
using Deepo.Client.Web.Navigation;
using System.Net;

namespace Deepo.Client.Web.Catalog;

public sealed class VinylLazyCatalog : IVinylCatalog, IVinylEventBusSubscriber, IDisposable
{
    private readonly IHttpService _httpService;
    private readonly IVinylEventBus _eventBus;

    private readonly List<ReleaseVinylDto> _releasesFetched = [];
    private List<ReleaseVinylDto> _releasesFiltered = [];
    private VinylFilterEventArgs? _filter;
    private Action? _onPropertyChanged;
    private int _nextOffset;

    public IReadOnlyList<ReleaseVinylDto> Items => _releasesFiltered;
    public bool IsLoaded { get; private set; }
    public bool IsInError { get; private set; }
    public string ErrorMessage { get; private set; }
    public bool CanGoNext { get; private set; }
    public int CurrentPageIndex { get; private set; }
    public int LastPageIndex => CalculateLastPageIndex();

    public VinylLazyCatalog(IHttpService httpService, IVinylEventBus eventBus)
    {
        _httpService = httpService;
        _eventBus = eventBus;
        _eventBus.Subscribe(this);

        this.CanGoNext = true;
        this.ErrorMessage = string.Empty;
    }

    public async Task NextAsync()
    {
        CurrentPageIndex++;
        await LoadPageAsync(CurrentPageIndex).ConfigureAwait(false);
        await LoadPageAsync(CurrentPageIndex + 1).ConfigureAwait(false);
    }

    public async Task PreviousAsync()
    {
        if (CurrentPageIndex > 1)
        {
            CurrentPageIndex--;
            await LoadPageAsync(CurrentPageIndex).ConfigureAwait(false);
        }
    }

    private async Task LoadPageAsync(int pageIndex)
    {
        int requiredItems = pageIndex * NavigationConst.ITEM_PER_PAGE;

        while (_releasesFiltered.Count < requiredItems && CanGoNext && !IsInError)
        {
            await LoadNextReleasesAsync().ConfigureAwait(false);
            ApplyFilter();
        }
    }

    private async Task LoadNextReleasesAsync()
    {
        string query = string.Format(CultureInfo.InvariantCulture, HttpRoute.VINYL_RELEASE_ROUTE, _nextOffset, NavigationConst.ITEM_PER_PAGE);
        OperationResult<string> httpResult = await _httpService.GetAsync(query, CancellationToken.None).ConfigureAwait(false);

        if (httpResult.ErrorCode == "204")
        {
            CanGoNext = false;
            return;
        }

        if (httpResult.IsFailed || !httpResult.HasContent)
        {
            IsInError = true;
            ErrorMessage = httpResult.ErrorMessage;
            return;
        }

        DtoResult<List<ReleaseVinylDto>>? operationResult = JsonConvert.DeserializeObject<DtoResult<List<ReleaseVinylDto>>>(httpResult.Content);
        if (operationResult == null || operationResult.Content == null || operationResult.IsFailed)
        {
            IsInError = true;
            ErrorMessage = operationResult?.ErrorMessage ?? string.Empty;  
            return;
        }

        _releasesFetched.AddRange(operationResult.Content);
        _nextOffset += operationResult.Content.Count;
        IsLoaded = true;
    }

    public async Task OnFilterChangedAsync(VinylFilterEventArgs args)
    {
        _filter = args;
        CurrentPageIndex = 1;
        ApplyFilter();
        await LoadPageAsync(CurrentPageIndex).ConfigureAwait(false);
    }

    private void ApplyFilter()
    {
        if (_filter?.SelectedGenres?.Any() == true)
        {
            _releasesFiltered = _releasesFetched
                .Where(release =>
                    release.Genres.Any(releaseGenre =>
                        _filter.SelectedGenres.Any(selectedGenre =>
                            selectedGenre.Identifier == releaseGenre.Identifier)))
                .ToList();
        }
        else
        {
            _releasesFiltered = _releasesFetched;
        }
        _onPropertyChanged?.Invoke();
    }

    private int CalculateLastPageIndex()
    {
        if (_releasesFiltered.Count == 0)
        {
            return 0;
        }

        int fullPages = _releasesFiltered.Count / NavigationConst.ITEM_PER_PAGE;
        return _releasesFiltered.Count % NavigationConst.ITEM_PER_PAGE > 0 ? fullPages + 1 : fullPages;
    }

    public void OnPropertyChanged(Action action)
    {
        _onPropertyChanged = action;
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe(this);
    }
}

public interface ICatalog
{
    bool IsLoaded { get; }
    bool IsInError { get; }
    string ErrorMessage { get; }

    bool CanGoNext { get; }
    int CurrentPageIndex { get; }
    int LastPageIndex { get; }

    Task NextAsync();
    Task PreviousAsync();
    void OnPropertyChanged(Action action);
}

public interface IVinylCatalog : ICatalog
{
    IReadOnlyList<ReleaseVinylDto> Items { get; }
}