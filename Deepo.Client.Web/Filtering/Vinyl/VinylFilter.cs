using Deepo.Client.Web.Dto;
using Deepo.Client.Web.Interfaces;
using System.Collections.ObjectModel;

namespace Deepo.Client.Web.Filtering.Vinyl;

public class VinylFilter : IFilter<ReleaseVinylDto>
{
    private readonly IVinylFiltersStore _filtersStore;
    private HashSet<Guid> _genresFilterCache;

    public event EventHandler<FilterEventArgs>? FilterChanged;
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