using Deepo.DAL.Repository.Feature.Fetcher;
using Deepo.Fetcher.Viewer.WPF;
using Deepo.Fetcher.Viewer.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using EF = Deepo.DAL.EF.Models;
using Deepo.Fetcher.Viewer.Models;
using Deepo.DAL.Repository.Interfaces;

namespace Deepo.Fetcher.Viewer.ViewModels;

/// <summary>
/// Provides detailed information and real-time monitoring for a selected fetcher, including execution status and activity logs.
/// </summary>
internal sealed class SelectedFetcherViewModel : BaseViewModel
{
    private readonly IFetcherListener _fetcherListener;
    private readonly IFetcherRepository _fetcherRepository;
    private EF.V_FetcherExtended? _model;

    /// <summary>
    /// Gets the collection of grid rows representing fetcher activity and results.
    /// </summary>
    public ObservableCollection<GridModel> FetcherRows { get; set; }

    /// <summary>
    /// Gets or sets the URI of the last request made by the fetcher.
    /// </summary>
    public string LastRequestedURI { get; set; }

    /// <summary>
    /// Gets a value indicating whether the fetcher is currently executing.
    /// </summary>
    public bool InExecution
    {
        get => _model?.LastStart != null && _model?.LastStart > _model?.LastEnd;
    }

    /// <summary>
    /// Gets the recurrence pattern or schedule type for the fetcher execution.
    /// </summary>
    public string Recurrence
    {
        get => _model?.PlanificationTypeName ?? "Unknow";
    }

    /// <summary>
    /// Gets the formatted date and time when the fetcher is scheduled to start next.
    /// </summary>
    public string StartAt
    {
        get => _model?.DateNextStart?.ToString(CultureInfo.CurrentCulture) ?? "n/a";
    }

    /// <summary>
    /// Gets the formatted date and time of the last fetcher execution.
    /// </summary>
    public string LastExecution
    {
        get => _model?.LastStart?.ToString(CultureInfo.CurrentCulture) ?? "n/a";
    }

    public SelectedFetcherViewModel(EF.Fetcher model,
        IFetcherRepository fetcherRepository,
        IFetcherListener fetcherListener)
    {
        _fetcherListener = fetcherListener;
        _fetcherRepository = fetcherRepository;

        _model = _fetcherRepository.GetExtendedAsync(model.Fetcher_GUID).Result;
        FetcherRows = [];
        LastRequestedURI = string.Empty;

        _fetcherListener.VinylReleaseRowAdded += OnVinylReleaseRowAdded;
        _fetcherListener.HttpRequestLogRowAdded += OnHttpRequestRowAdded;
        _fetcherListener.FetcherExecutionRowAdded += OnFetcherExecutionRowAdded;
    }

    private void OnVinylReleaseRowAdded(object? sender, GridModelEventArgs e)
    {
        App.Current.Dispatcher.BeginInvoke(() =>
        {
            FetcherRows.Add(e.Row);
        });
    }

    private void OnHttpRequestRowAdded(object? sender, HttpRequestLogEventArgs e)
    {
        App.Current.Dispatcher.BeginInvoke(() =>
        {
            LastRequestedURI = e.Request;
            OnPropertyChanged(nameof(LastRequestedURI));
        });
    }

    private async void OnFetcherExecutionRowAdded(object? sender, FetcherExecutionEventArgs e)
    {
        await App.Current.Dispatcher.BeginInvoke(async () =>
        {
            if (_model?.Fetcher_GUID == e.FetcherIdentifier)
            {
                _model = await _fetcherRepository.GetExtendedAsync(e.FetcherIdentifier).ConfigureAwait(false);
                OnPropertyChanged(nameof(InExecution));
                OnPropertyChanged(nameof(StartAt));
                OnPropertyChanged(nameof(LastExecution));
            }
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if ( _fetcherListener != null)
            {
                _fetcherListener.VinylReleaseRowAdded -= OnVinylReleaseRowAdded;
                _fetcherListener.HttpRequestLogRowAdded -= OnHttpRequestRowAdded;
                _fetcherListener.FetcherExecutionRowAdded -= OnFetcherExecutionRowAdded;
                _fetcherListener.Dispose();
            }
            this.FetcherRows?.Clear();
        }
        base.Dispose(disposing);
    }
}