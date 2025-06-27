using Deepo.Client.Web.Dto;
using Deepo.Client.Web.Interfaces;
using System.Collections.ObjectModel;

namespace Deepo.Client.Web.Filtering.Vinyl;

public class VinylFilter : IFilter<ReleaseVinylDto>
{
    private readonly IVinylFiltersStore _filtersStore;

    public event EventHandler? FilterChanged;
    public Collection<Func<ReleaseVinylDto, bool>> Predicates => [
        FilterByReleaseDate,
        FilterByGenre
    ];

    public VinylFilter(IVinylFiltersStore vinylFiltersStore)
    {
        _filtersStore = vinylFiltersStore;
        _filtersStore.FilterChanged += (e, r) =>
        {
            FilterChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    private bool FilterByGenre(ReleaseVinylDto release)
    {
        return release.Genres.Any(releaseGenre =>
            _filtersStore.SearchedGenres.Any(selectedGenre =>
                selectedGenre.Identifier == releaseGenre.Identifier));
    }

    private bool FilterByReleaseDate(ReleaseVinylDto release)
    {
        return release.ReleaseDate.Month == _filtersStore.SearchDate.ToUniversalTime().Month;
    }
}