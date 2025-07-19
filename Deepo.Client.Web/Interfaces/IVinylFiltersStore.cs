using Deepo.Client.Web.Dto;
using Deepo.Client.Web.Filtering;

namespace Deepo.Client.Web.Interfaces;

/// <summary>
/// Defines the contract for managing filter criteria for vinyl releases.
/// </summary>
public interface IVinylFiltersStore
{
    /// <summary>
    /// Asynchronously initializes the filter store with available filter options.
    /// </summary>
    Task InitializeAsync();
    
    /// <summary>
    /// Gets the currently selected search date for filtering releases.
    /// </summary>
    DateTime SearchDate { get; }
    
    /// <summary>
    /// Gets the collection of genres currently selected for filtering.
    /// </summary>
    IEnumerable<GenreDto> SearchedGenres { get; }
    
    /// <summary>
    /// Gets the collection of all available genres that can be used for filtering.
    /// </summary>
    IEnumerable<GenreDto> AvailableGenres { get; }
    
    /// <summary>
    /// Gets the collection of markets currently selected for filtering.
    /// </summary>
    IEnumerable<string> SearchedMarkets { get; }
    
    /// <summary>
    /// Gets the collection of all available markets that can be used for filtering.
    /// </summary>
    IEnumerable<string> AvailableMarkets { get; }

    /// <summary>
    /// Occurs when filter criteria have changed.
    /// </summary>
    event EventHandler<FilterEventArgs> FilterChanged;

    /// <summary>
    /// Sets the genres to be used for filtering vinyl releases.
    /// </summary>
    void SetSearchedGenres(IEnumerable<GenreDto> genres);
    
    /// <summary>
    /// Sets the markets to be used for filtering vinyl releases.
    /// </summary>
    void SetSearchedMarkets(IEnumerable<string> markets);
    
    /// <summary>
    /// Sets the date to be used for filtering vinyl releases.
    /// </summary>
    void SetSearchedDate(DateTime date);
}
