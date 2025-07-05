using Android.Views;
using AndroidX.RecyclerView.Widget;
using Deepo.Client.App.Adapter;
using Deepo.Framework.Interfaces;
using Deepo.Framework.Results;
using Newtonsoft.Json;

namespace Deepo.Client.App.Fragments
{
    public class NewReleasesFragment : AndroidX.Fragment.App.Fragment
    {
        //Dependencies
        private readonly IHttpService _httpService;

        //
        private NewReleasesAdapter? _newReleasesAdapter;
        private LinearLayoutManager? _layoutManager;

        //UI
        RecyclerView? _recyclerViewNewReleases;

        public NewReleasesFragment(IHttpService httpService)
        {
            _httpService = httpService;

        }

        public override View? OnCreateView(LayoutInflater? inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            View? view = inflater?.Inflate(Resource.Layout.layout_newReleases, container, false);
            _recyclerViewNewReleases = view?.FindViewById<RecyclerView>(Resource.Id.recyclerViewNewReleases);
            _layoutManager = new LinearLayoutManager(this.Context);
            _recyclerViewNewReleases?.SetLayoutManager(_layoutManager);

            Task<OperationResult<string>> task = Task.Run(async () =>
            {
                return await _httpService.GetAsync("vinyl?market=FR&offset=100&limit=50", CancellationToken.None).ConfigureAwait(false);
            });
            var ee = task.Result;
            if (ee != null)
            {
                Dto.DtoResult<List<Dto.ReleaseVinylDto>>? result = JsonConvert.DeserializeObject<Dto.DtoResult<List<Dto.ReleaseVinylDto>>>(ee.Content);
                if (result != null && result.Content != null)
                {
                    _newReleasesAdapter = new NewReleasesAdapter(result.Content);
                }
                _recyclerViewNewReleases?.SetAdapter(_newReleasesAdapter);
            }

            return view;
        }

        private void _button_Click(object? sender, EventArgs e)
        {


        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _newReleasesAdapter?.Dispose();
                _layoutManager?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
