using System.Collections.Specialized;
using System.ComponentModel;

namespace Deepo.Client.Web.Interfaces;

/// <summary>
/// Defines the contract for a collection that can be filtered and provides change notifications.
/// </summary>
public interface IFilteredCollection<T> : INotifyCollectionChanged, INotifyPropertyChanged, IList<T> where T : class
{
    /// <summary>
    /// Gets or sets the filter applied to this collection.
    /// </summary>
    public IFilter<T>? Filter { get; set; }
}