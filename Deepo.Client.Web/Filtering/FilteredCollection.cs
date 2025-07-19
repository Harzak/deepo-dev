using Deepo.Client.Web.Interfaces;
using System.Collections.ObjectModel;

namespace Deepo.Client.Web.Filtering;

/// <summary>
/// Represents an observable collection that can be filtered based on specified criteria.
/// </summary>
public sealed class FilteredCollection<T> : ObservableCollection<T>, IFilteredCollection<T> , IDisposable where T : class
{
    private readonly List<T> _allItems;
    private IFilter<T>? _filter;
    private bool _isFiltering;

    /// <summary>
    /// Gets or sets the filter applied to this collection.
    /// </summary>
    public IFilter<T>? Filter
    {
        get => _filter;
        set
        {
            if (_filter != null)
            {
                _filter.FilterChanged -= OnFilterChanged;
            }
            _filter = value;
            if (_filter != null)
            {
                _filter.FilterChanged += OnFilterChanged;
            }
            ApplyFilters();
        }
    }

    public FilteredCollection()
    {
        _allItems = [];
    }

    public FilteredCollection(IEnumerable<T> values) : base(values)
    {
        _allItems = [.. values];
    }

    protected override void InsertItem(int index, T item)
    {
        if (!_isFiltering)
        {
            _allItems.Add(item);
            if (!MatchesFilters(item))
            {
                return;
            }
        }
        base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        if (!_isFiltering)
        {
            T item = this[index];
            _allItems.Remove(item);
        }
        base.RemoveItem(index);
    }

    protected override void ClearItems()
    {
        if (!_isFiltering)
        {
            _allItems.Clear();
        }
        base.ClearItems();
    }

    private bool MatchesFilters(T item)
    {
        return _filter == null || _filter.Predicates.Count == 0 || _filter.Predicates.All(filter => filter(item));
    }

    private void ApplyFilters()
    {
        if (_isFiltering)
        {
            return;
        }

        _isFiltering = true;
        HashSet<T> currentVisibleItems = [.. this];
        List<T> itemsToAdd = [];
        List<T> itemsToRemove = [];

        foreach (T item in _allItems)
        {
            bool shouldBeVisible = MatchesFilters(item);
            bool isCurrentlyVisible = currentVisibleItems.Contains(item);

            if (shouldBeVisible && !isCurrentlyVisible)
            {
                itemsToAdd.Add(item);
            }
            else if (!shouldBeVisible && isCurrentlyVisible)
            {
                itemsToRemove.Add(item);
            }
        }

        foreach (T item in itemsToRemove)
        {
            int index = this.IndexOf(item);
            if (index >= 0)
            {
                base.RemoveItem(index);
            }
        }

        int insertIndex = 0;
        foreach (T item in _allItems.Where(MatchesFilters))
        {
            if (itemsToAdd.Contains(item))
            {
                base.InsertItem(insertIndex, item);
            }

            if (this.Count > insertIndex && this[insertIndex] == item)
            {
                insertIndex++;
            }
        }
        _isFiltering = false;
    }

    private void OnFilterChanged(object? sender, FilterEventArgs e)
    {
        ApplyFilters();
    }

    public void Dispose()
    {
        if (_filter != null)
        {
            _filter.FilterChanged -= OnFilterChanged;
            _filter = null;
        }
        _allItems.Clear();
        ClearItems();
    }
}
