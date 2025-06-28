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

namespace Deepo.Fetcher.Viewer.ViewModels
{
    internal sealed class SelectedFetcherViewModel : ViewModelBase
    {
        private readonly EF.V_FetcherExtended? _model;
        private readonly IFetcherListener _fetcherListener;

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

        public SelectedFetcherViewModel(EF.Fetcher model, 
            IFetcherRepository fetcherRepository, 
            IFetcherListener fetcherListener)
        {
            _fetcherListener = fetcherListener;
            _fetcherListener.VinylReleaseRowAdded += FetcherGridProvider_RowAdded;
            _fetcherListener.HttpRequestRowAdded += FetcherHttpRequestProvider_RowAdded;
            _model = fetcherRepository.GetExtended(model.Fetcher_GUID);

            FetcherRows = [];
            LastRequestedURI = string.Empty;
            _fetcherListener.StartListener();
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
            _fetcherListener.Dispose();
        }
    }
}
