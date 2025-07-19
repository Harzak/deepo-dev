using Deepo.Client.Web.Dto;

namespace Deepo.Client.Web.Filtering;

/// <summary>
/// Provides data for filter change events, containing information about the current filter criteria.
/// </summary>
public class FilterEventArgs : EventArgs
{
    /// <summary>
    /// Gets the date used for filtering releases.
    /// </summary>
    public DateTime SearchDate { get; }
    
    /// <summary>
    /// Gets the collection of genres currently selected for filtering.
    /// </summary>
    public IEnumerable<GenreDto> SearchedGenres { get; }
    
    /// <summary>
    /// Gets the collection of markets currently selected for filtering.
    /// </summary>
    public IEnumerable<string> SearchedMarkets { get; }

    public FilterEventArgs(DateTime searchDate, IEnumerable<GenreDto> searchedGenres, IEnumerable<string> searchedMarkets)
    {
        SearchDate = searchDate;
        SearchedGenres = searchedGenres;
        SearchedMarkets = searchedMarkets;
    }
}

