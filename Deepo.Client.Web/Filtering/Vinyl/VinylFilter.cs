using Deepo.Client.Web.Dto;
using Deepo.Client.Web.Interfaces;
using System.Collections.ObjectModel;

namespace Deepo.Client.Web.Filtering.Vinyl;

/// <summary>
/// Provides filtering capabilities for vinyl release collections.
/// </summary>
public class VinylFilter : IFilter<ReleaseVinylDto>
{
    private readonly IVinylFiltersStore _filtersStore;
    private HashSet<Guid> _genresFilterCache;

    /// <summary>
    /// Occurs when filter criteria have changed.
    /// </summary>
    public event EventHandler<FilterEventArgs>? FilterChanged;
    
    /// <summary>
    /// Gets the collection of filter predicates used to determine which items should be visible.
    /// </summary>
    public Collection<Func<ReleaseVinylDto, bool>> Predicates => [
        FilterByReleaseDate,
        FilterByGenre
    ];

    public VinylFilter(IVinylFiltersStore vinylFiltersStore)
    {
        _filtersStore = vinylFiltersStore;
        _filtersStore.FilterChanged += OnFiltersChanged;
        _genresFilterCache = [];
        UpdateFilterCache();
    }

    private void OnFiltersChanged(object? sender, FilterEventArgs args)
    {
        UpdateFilterCache();
        FilterChanged?.Invoke(this, args);
    }

    private void UpdateFilterCache()
    {
        UpdateGenreCache();
    }

    private bool FilterByGenre(ReleaseVinylDto release)
    {
        return release.Genres.Any(releaseGenre => _genresFilterCache.Contains(releaseGenre.Identifier));
    }

    private void UpdateGenreCache()
    {
        _genresFilterCache = _filtersStore.SearchedGenres.Select(g => g.Identifier).ToHashSet();
    }

    private bool FilterByReleaseDate(ReleaseVinylDto release)
    {
        return release.ReleaseDate.Month == _filtersStore.SearchDate.Month;
    }
}