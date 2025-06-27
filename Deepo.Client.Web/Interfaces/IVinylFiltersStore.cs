using Deepo.Client.Web.Dto;

namespace Deepo.Client.Web.Interfaces;

public interface IVinylFiltersStore
{
    Task InitializeAsync();
    DateTime SearchDate { get; }
    IReadOnlyCollection<GenreDto> SearchedGenres { get; }
    IReadOnlyCollection<GenreDto> AvailableGenres { get; }
    IReadOnlyCollection<string> SearchedMarkets { get; }
    IReadOnlyCollection<string> AvailableMarkets { get; }

    event EventHandler FilterChanged;

    void SetSearchedGenres(IEnumerable<GenreDto> genres);
    void SetSearchedMarkets(IEnumerable<string> markets);
    void SetSearchedDate(DateTime date);
}
