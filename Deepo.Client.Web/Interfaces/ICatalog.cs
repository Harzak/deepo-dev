namespace Deepo.Client.Web.Interfaces;

/// <summary>
/// Defines the contract for a catalog that supports pagination and loading states.
/// </summary>
public interface ICatalog
{
    /// <summary>
    /// Gets a value indicating whether the catalog has been loaded with data.
    /// </summary>
    bool IsLoaded { get; }
    
    /// <summary>
    /// Gets a value indicating whether an error occurred during catalog operations.
    /// </summary>
    bool IsInError { get; }
    
    /// <summary>
    /// Gets the error message when an error occurs during catalog operations.
    /// </summary>
    string ErrorMessage { get; }

    /// <summary>
    /// Gets a value indicating whether more items can be loaded from the data source.
    /// </summary>
    bool CanGoNext { get; }
    
    /// <summary>
    /// Gets the current page index in the paginated catalog.
    /// </summary>
    int CurrentPageIndex { get; }
    
    /// <summary>
    /// Gets the index of the last page based on the current loaded items.
    /// </summary>
    int LastPageIndex { get; }

    /// <summary>
    /// Asynchronously loads the next page of items.
    /// </summary>
    Task NextAsync();
    
    /// <summary>
    /// Asynchronously loads the previous page of items.
    /// </summary>
    Task PreviousAsync();
}
