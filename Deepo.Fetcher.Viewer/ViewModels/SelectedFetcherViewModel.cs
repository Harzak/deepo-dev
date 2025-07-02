using Deepo.DAL.Repository.Feature.Fetcher;
using Deepo.Fetcher.Host.WPF;
using Deepo.Fetcher.Viewer.Interfaces;
using Framework.WPF.Behavior.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using EF = Deepo.DAL.EF.Models;
using Deepo.Fetcher.Viewer.Models;
using Deepo.DAL.Repository.Interfaces;

namespace Deepo.Fetcher.Viewer.ViewModels;

internal sealed class SelectedFetcherViewModel : ViewModelBase
{
    private readonly IFetcherListener _fetcherListener;
    private readonly IFetcherRepository _fetcherRepository;
    private EF.V_FetcherExtended? _model;

    public ObservableCollection<GridModel> FetcherRows { get; set; }

    public string LastRequestedURI { get; set; }

    public bool InExecution
    {
        get => _model?.LastStart != null && _model?.LastStart > _model?.LastEnd;
    }

    public string Recurrence
    {
        get => _model?.PlanificationTypeName ?? "Unknow";
    }

    public string StartAt
    {
        get => _model?.DateNextStart?.ToString(CultureInfo.CurrentCulture) ?? "n/a";
    }

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
        base.Dispose(disposing);
        _fetcherListener.Dispose();
    }
}