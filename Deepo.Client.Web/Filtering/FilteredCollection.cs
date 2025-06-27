using Deepo.Client.Web.Interfaces;
using System.Collections.ObjectModel;

namespace Deepo.Client.Web.Filtering;

public sealed class FilteredCollection<T> : ObservableCollection<T>, IFilteredCollection<T> where T : class
{
    private readonly List<T> _allItems;
    private IFilter<T>? _filter;
    private bool _isFiltering;

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
        if (_isFiltering) return;

        _isFiltering = true;
        try
        {
            base.ClearItems();

            foreach (T item in _allItems.Where(MatchesFilters))
            {
                base.InsertItem(base.Count, item);
            }
        }
        finally
        {
            _isFiltering = false;
        }
    }

    private void OnFilterChanged(object? sender, EventArgs e)
    {
        ApplyFilters();
    }
}
