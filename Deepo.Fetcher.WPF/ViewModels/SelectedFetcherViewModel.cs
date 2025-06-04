using Deepo.DAL.Service.Feature.Fetcher;
using Deepo.Fetcher.Host.WPF;
using Deepo.Fetcher.WPF.Features.FetcherGrid.Model;
using Deepo.Fetcher.WPF.Features.FetcherGrid.Provider;
using Framework.WPF.Behavior.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.Fetcher.WPF.ViewModels
{
    internal sealed class SelectedFetcherViewModel : ViewModelBase
    {
        private readonly Models.V_FetcherExtended? _model;
        private readonly IFetcherGridProvider _fetcherGridProvider;

        public ObservableCollection<GridModel> FetcherRows { get; set; }

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

        public SelectedFetcherViewModel(Models.Fetcher model, IFetcherDBService fetcherDBService, IFetcherGridProvider fetcherGridProvider)
        {
            _fetcherGridProvider = fetcherGridProvider;
            _fetcherGridProvider.RowAdded += FetcherGridProvider_RowAdded;
            _model = fetcherDBService.GetExtended(model.Fetcher_GUID);

            FetcherRows = [];
        }

        private void FetcherGridProvider_RowAdded(object? sender, GridModelEventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(() =>
            {
                FetcherRows.Add(e.Row);
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _fetcherGridProvider.Dispose();
        }
    }
}
