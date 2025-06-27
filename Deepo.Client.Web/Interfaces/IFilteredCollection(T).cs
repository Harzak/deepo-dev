using System.Collections.Specialized;
using System.ComponentModel;

namespace Deepo.Client.Web.Interfaces;

public interface IFilteredCollection<T> : INotifyCollectionChanged, INotifyPropertyChanged, IList<T> where T : class
{
    public IFilter<T>? Filter { get; set; }
}