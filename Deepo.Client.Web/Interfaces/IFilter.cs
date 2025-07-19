using Deepo.Client.Web.Catalog;
using Deepo.Client.Web.Filtering;
using System.Collections.ObjectModel;

namespace Deepo.Client.Web.Interfaces;

/// <summary>
/// Defines the contract for filtering collections of items based on specific criteria.
/// </summary>
public interface IFilter<T> where T : class
{
    /// <summary>
    /// Occurs when filter criteria have changed.
    /// </summary>
    public event EventHandler<FilterEventArgs>? FilterChanged;
    
    /// <summary>
    /// Gets the collection of filter predicates used to determine which items should be visible.
    /// </summary>
    public Collection<Func<T, bool>> Predicates { get; }
}