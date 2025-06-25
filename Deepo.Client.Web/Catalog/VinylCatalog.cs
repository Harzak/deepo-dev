using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Dto;
using Deepo.Client.Web.EventBus.Vinyl;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Newtonsoft.Json;
using System.Globalization;

namespace Deepo.Client.Web.Catalog;

public sealed class VinylCatalog : IVinylCatalog, IVinylEventBusSubscriber, IDisposable
{
    private const int ITEM_PER_PAGE = 4;

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
    public bool CanGoNext { get; private set; }
    public int CurrentPageIndex { get; set; }
    public int LastPageIndex => CalculateLastPageIndex();

    public VinylCatalog(IHttpService httpService, IVinylEventBus eventBus)
    {
        _httpService = httpService;
        _eventBus = eventBus;
        _eventBus.Subscribe(this);

        this.CanGoNext = true;
    }

    public async Task GoNext()
    {
        CurrentPageIndex++;
        await LoadPageAsync(CurrentPageIndex).ConfigureAwait(false);
    }

    private async Task LoadPageAsync(int pageIndex)
    {
        int requiredItems = pageIndex * ITEM_PER_PAGE;

        while (_releasesFiltered.Count < requiredItems && CanGoNext)
        {
            await LoadNextReleasesAsync().ConfigureAwait(false);
            ApplyFilter();
            _onPropertyChanged?.Invoke();

            if (!CanGoNext && _releasesFiltered.Count < requiredItems)
            {
                break;
            }
        }
    }

    private async Task LoadNextReleasesAsync()
    {
        string query = string.Format(CultureInfo.InvariantCulture, HttpRoute.VINYL_RELEASE_ROUTE, _nextOffset, ITEM_PER_PAGE);
        OperationResult<string> httpResult = await _httpService.GetAsync(query, CancellationToken.None).ConfigureAwait(false);

        if (httpResult.IsFailed || !httpResult.HasContent)
        {
            IsInError = true;
            return;
        }

        DtoResult<List<ReleaseVinylDto>>? operationResult = JsonConvert.DeserializeObject<DtoResult<List<ReleaseVinylDto>>>(httpResult.Content);
        if (operationResult == null || operationResult.Content == null || operationResult.IsFailed)
        {
            IsInError = true;
            return;
        }

        if (operationResult.Content.Count == 0)
        {
            CanGoNext = false;
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
    }

    private int CalculateLastPageIndex()
    {
        if (_releasesFiltered.Count == 0)
        {
            return 0;
        }

        int fullPages = _releasesFiltered.Count / ITEM_PER_PAGE;
        return _releasesFiltered.Count % ITEM_PER_PAGE > 0 ? fullPages + 1 : fullPages;
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
    bool CanGoNext { get; }
    Task GoNext();
    void OnPropertyChanged(Action action);
}

public interface IVinylCatalog : ICatalog
{
    IReadOnlyList<ReleaseVinylDto> Items { get; }
}