using Deepo.Client.Web.Catalog;
using Deepo.Client.Web.Filtering;
using System.Collections.ObjectModel;

namespace Deepo.Client.Web.Interfaces;

public interface IFilter<T> where T : class
{
    public event EventHandler? FilterChanged;
    public Collection<Func<T, bool>> Predicates { get; }
}