using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Dto;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Newtonsoft.Json;
using System.Globalization;
using Deepo.Client.Web.Filtering;
using Deepo.Client.Web.Interfaces;

namespace Deepo.Client.Web.Catalog;

public sealed class VinylLazyCatalog : IVinylCatalog
{
    private readonly IHttpService _httpService;
    private int _nextOffset;

    public IFilteredCollection<ReleaseVinylDto> Items {get;set;}
    public bool IsLoaded { get; private set; }
    public bool IsInError { get; private set; }
    public string ErrorMessage { get; private set; }
    public bool CanGoNext { get; private set; }
    public int CurrentPageIndex { get; private set; }
    public int LastPageIndex => CalculateLastPageIndex();

    public VinylLazyCatalog(IHttpService httpService, IFilter<ReleaseVinylDto> filter)
    {
        _httpService = httpService;
        Items = new FilteredCollection<ReleaseVinylDto>()
        {
            Filter = filter,
        };

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
        int requiredItems = pageIndex * Constants.RELEASES_PER_PAGE;

        while (Items.Count < requiredItems && CanGoNext && !IsInError)
        {
            await LoadNextReleasesAsync().ConfigureAwait(false);
        }
    }

    private async Task LoadNextReleasesAsync()
    {
        string query = string.Format(CultureInfo.InvariantCulture, HttpRoute.VINYL_RELEASE_ROUTE, _nextOffset, Constants.RELEASES_PER_PAGE);
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

        operationResult.Content.ForEach(Items.Add);
        _nextOffset += operationResult.Content.Count;
        IsLoaded = true;
    }

    private int CalculateLastPageIndex()
    {
        if (!Items.Any())
        {
            return 0;
        }

        int fullPages = Items.Count / Constants.RELEASES_PER_PAGE;
        return Items.Count % Constants.RELEASES_PER_PAGE > 0 ? fullPages + 1 : fullPages;
    }
}