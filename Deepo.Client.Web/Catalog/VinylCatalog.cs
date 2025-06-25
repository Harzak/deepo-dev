using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Dto;
using Deepo.Client.Web.EventBus.Vinyl;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using MudBlazor;
using Newtonsoft.Json;
using System.Globalization;

namespace Deepo.Client.Web.Catalog;

public sealed class VinylCatalog : IVinylCatalog, IVinylEventBusSubscriber, IDisposable
{
    private const int ITEM_PER_PAGE = 4;

    private readonly IHttpService _httpService;
    private readonly IVinylEventBus _eventBus;

    private List<ReleaseVinylDto> _releasesFiltered;
    private List<ReleaseVinylDto> _releasesFetched;
    private VinylFilterEventArgs? _filter;
    private Action? _onPropertyChanged;
    private int _nextOffset;

    public IReadOnlyList<ReleaseVinylDto> Items => _releasesFiltered;
    public bool IsLoaded { get; private set; }
    public bool IsInError { get; private set; }
    public bool HasContent => _releasesFiltered.Count > 0;
    public bool CanGetNext { get; private set; }
    public int CurrentPageIndex { get; set; }  
    public int LastPageIndex => _releasesFiltered.Count / ITEM_PER_PAGE + (_releasesFiltered.Count % ITEM_PER_PAGE > 0 ? 1 : 0);  

    public VinylCatalog(IHttpService httpService, IVinylEventBus eventBus)
    {
        _httpService = httpService;
        _eventBus = eventBus;

        _releasesFetched = [];
        _releasesFiltered = [];
        _eventBus.Subscribe(this);
        CanGetNext = true;
        CurrentPageIndex = 0;
    }

    public async Task GoNextPage()
    {
        CurrentPageIndex++;
        await this.LoadReleasesAsync().ConfigureAwait(false);
        await this.ApplyFiltersAsync().ConfigureAwait(false);

    }

    public async Task LoadReleasesAsync()
    {
        List<ReleaseVinylDto>? lastFetch = await GetReleasesAsync(_nextOffset, limit: ITEM_PER_PAGE).ConfigureAwait(false);
        if (this.IsInError || lastFetch == null)
        {
            return;
        }
        else if (lastFetch.Count == 0)
        {
            CanGetNext = false;
        }
        else
        {
            _releasesFetched.AddRange(lastFetch);
            _nextOffset += lastFetch.Count;   
        }
    }

    private async Task<List<ReleaseVinylDto>?> GetReleasesAsync(int offset, int limit)
    {
        string query = string.Format(CultureInfo.InvariantCulture, HttpRoute.VINYL_RELEASE_ROUTE, offset, limit);

        OperationResult<string> httpResult;
        try
        {
            httpResult = await _httpService.GetAsync(query, CancellationToken.None).ConfigureAwait(false);
        }
        catch (HttpRequestException)
        {
            this.IsInError = true;
            return null;
        }
        catch (Exception)
        {
            throw;
        }

        if (httpResult.IsSuccess && httpResult.HasContent)
        {
            DtoResult<List<ReleaseVinylDto>>? operationResult = JsonConvert.DeserializeObject<DtoResult<List<ReleaseVinylDto>>>(httpResult.Content);
            if (operationResult?.Content != null && operationResult.IsSuccess)
            {
                this.IsLoaded = true;
                return operationResult.Content;
            }
            else
            {
                return [];
            }
        }
        this.IsInError = true;
        return null;
    }

    public async Task OnFilterChangedAsync(VinylFilterEventArgs args)
    {
        _filter = args;
        await this.ApplyFiltersAsync().ConfigureAwait(false);
    }

    private async Task ApplyFiltersAsync()
    {
        ApplyFiltersInternal();
        await CompletePage(this.CurrentPageIndex).ConfigureAwait(false);  
        _onPropertyChanged?.Invoke();
    }

    private void ApplyFiltersInternal()
    {
        if (_filter != null)
        {
            _releasesFiltered = _releasesFetched.Where(release => _filter.SelectedGenres.Any(genre => release.Genres.Any(x => x.Identifier == genre.Identifier))).ToList() ?? [];
        }
        else
        {
            _releasesFiltered = _releasesFetched;
        }
    }

    private async Task CompletePage(int page)
    {
        while (_releasesFiltered.Count < page * ITEM_PER_PAGE && CanGetNext)
        {
            await LoadReleasesAsync().ConfigureAwait(false);
            ApplyFiltersInternal();
        }
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
    bool HasContent { get; }
    bool CanGetNext { get; }
    Task GoNextPage();
    void OnPropertyChanged(Action action);
}

public interface IVinylCatalog : ICatalog
{
    IReadOnlyList<ReleaseVinylDto> Items { get; }
}