using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Dto;
using Newtonsoft.Json;
using System.Globalization;
using Deepo.Client.Web.Filtering;
using Deepo.Client.Web.Interfaces;
using Deepo.Framework.Results;
using Deepo.Framework.Interfaces;

namespace Deepo.Client.Web.Catalog;

/// <summary>
/// Provides a lazy-loading catalog implementation for vinyl releases with pagination and filtering capabilities.
/// </summary>
public sealed class VinylLazyCatalog : IVinylCatalog
{
    private readonly IHttpService _httpService;
    private int _nextOffset;

    /// <summary>
    /// Gets or sets the filtered collection of vinyl release items.
    /// </summary>
    public IFilteredCollection<ReleaseVinylDto> Items { get; set; }

    /// <summary>
    /// Gets a value indicating whether the catalog has been loaded with data.
    /// </summary>
    public bool IsLoaded { get; private set; }

    /// <summary>
    /// Gets a value indicating whether an error occurred during catalog operations.
    /// </summary>
    public bool IsInError { get; private set; }

    /// <summary>
    /// Gets the error message when an error occurs during catalog operations.
    /// </summary>
    public string ErrorMessage { get; private set; }

    /// <summary>
    /// Gets a value indicating whether more items can be loaded from the data source.
    /// </summary>
    public bool CanGoNext { get; private set; }

    /// <summary>
    /// Gets the current page index in the paginated catalog.
    /// </summary>
    public int CurrentPageIndex { get; private set; }

    /// <summary>
    /// Gets the index of the last page based on the current loaded items.
    /// </summary>
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

    /// <summary>
    /// Asynchronously loads the next page of vinyl releases.
    /// </summary>
    public async Task NextAsync()
    {
        CurrentPageIndex++;
        await LoadPageAsync(CurrentPageIndex).ConfigureAwait(false);
        await LoadPageAsync(CurrentPageIndex + 1).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously loads the previous page of vinyl releases.
    /// </summary>
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