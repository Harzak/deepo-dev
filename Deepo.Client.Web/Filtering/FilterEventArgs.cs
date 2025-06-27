using Deepo.Client.Web.Dto;

namespace Deepo.Client.Web.Filtering;

public class FilterEventArgs : EventArgs
{
    public DateTime SearchDate { get; }
    public IEnumerable<GenreDto> SearchedGenres { get; }
    public IEnumerable<string> SearchedMarkets { get; }

    public FilterEventArgs(DateTime searchDate, IEnumerable<GenreDto> searchedGenres, IEnumerable<string> searchedMarkets)
    {
        SearchDate = searchDate;
        SearchedGenres = searchedGenres;
        SearchedMarkets = searchedMarkets;
    }
}

