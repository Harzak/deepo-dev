using Deepo.Client.Web.Dto;
using Deepo.Client.Web.Filtering;

namespace Deepo.Client.Web.Interfaces;

public interface IVinylFiltersStore
{
    Task InitializeAsync();
    DateTime SearchDate { get; }
    IEnumerable<GenreDto> SearchedGenres { get; }
    IEnumerable<GenreDto> AvailableGenres { get; }
    IEnumerable<string> SearchedMarkets { get; }
    IEnumerable<string> AvailableMarkets { get; }

    event EventHandler<FilterEventArgs> FilterChanged;

    void SetSearchedGenres(IEnumerable<GenreDto> genres);
    void SetSearchedMarkets(IEnumerable<string> markets);
    void SetSearchedDate(DateTime date);
}
