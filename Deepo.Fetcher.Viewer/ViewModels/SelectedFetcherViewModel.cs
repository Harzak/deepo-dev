using Deepo.DAL.Service.Feature.Fetcher;
using Deepo.Fetcher.Host.WPF;
using Deepo.Fetcher.Viewer.Interfaces;
using Deepo.Fetcher.Viewer.Features.FetcherGrid.Model;
using Framework.WPF.Behavior.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.Fetcher.Viewer.ViewModels
{
    internal sealed class SelectedFetcherViewModel : ViewModelBase
    {
        private readonly Models.V_FetcherExtended? _model;
        private readonly IFetcherGridProvider _fetcherGridProvider;
        private readonly IFetcherHttpRequestProvider _httpRequestProvider;

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
            get => (_model?.DateNextStart ?? DateTime.MinValue).ToString(CultureInfo.CurrentCulture);
        }

        public string LastExecution
        {
            get => (_model?.LastStart ?? DateTime.MinValue).ToString(CultureInfo.CurrentCulture);
        }

        public SelectedFetcherViewModel(Models.Fetcher model, 
            IFetcherDBService fetcherDBService, 
            IFetcherGridProvider fetcherGridProvider,
            IFetcherHttpRequestProvider httpRequestProvider)
        {
            _fetcherGridProvider = fetcherGridProvider;
            _httpRequestProvider = httpRequestProvider;
            _fetcherGridProvider.RowAdded += FetcherGridProvider_RowAdded;
            _httpRequestProvider.RowAdded += FetcherHttpRequestProvider_RowAdded;
            _model = fetcherDBService.GetExtended(model.Fetcher_GUID);

            FetcherRows = [];
            LastRequestedURI = string.Empty;
        }

        private void FetcherGridProvider_RowAdded(object? sender, GridModelEventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(() =>
            {
                FetcherRows.Add(e.Row);
            });
        }

        private void FetcherHttpRequestProvider_RowAdded(object? sender, string e)
        {
            App.Current.Dispatcher.BeginInvoke(() =>
            {
                LastRequestedURI = e;
                OnPropertyChanged(nameof(LastRequestedURI));
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _fetcherGridProvider.Dispose();
        }
    }
}
