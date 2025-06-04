using AndroidX.AppCompat.App;
using Deepo.Client.AndroidApp.DI;
using Deepo.Client.AndroidApp.Navigation;
using Deepo.Client.AndroidApp.Navigation.Interfaces;
using Google.Android.Material.BottomNavigation;
using Ninject;
using Ninject.Modules;

namespace Deepo.Client.AndroidApp.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, INavigationListener
    {
        private readonly StandardKernel _dependenciesContainer;
        private readonly INavigationService _navigationService;

        private BottomNavigationView? _navigationBar;

        public MainActivity()
        {
            _dependenciesContainer = new StandardKernel(new INinjectModule[] {
                new LoggingModule(),
                new UtilsModule(),
                new HttpModule(),
                new NavigationModule(),
            });
            _navigationService = _dependenciesContainer.Get<INavigationServiceFactory>().CreateNavigationService(base.SupportFragmentManager, this);
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            _navigationBar = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            if (_navigationBar != null)
            {
                _navigationBar.ItemSelected += OnNavigationBarItemSelected;
            }
            _navigationService.NavigateToHome();
        }

        private void OnNavigationBarItemSelected(object? sender, Google.Android.Material.Navigation.NavigationBarView.ItemSelectedEventArgs e)
        {
            _navigationService.NavigateTo(e.Item);
        }

        public void OnNavigatedBack(int layoutId)
        {
            SelectNavigationBarItem(layoutId);
        }

        public void OnNavigated(int layoutId)
        {
            SelectNavigationBarItem(layoutId);
        }

        public override void OnBackPressed()
        {
            _navigationService?.NavigateBack(this);
        }

        private void SelectNavigationBarItem(int layoutId)
        {
            int menuItemId = Pages.Catalog.FirstOrDefault(x => x.Value == layoutId).Key;
            int? selectedId = _navigationBar?.SelectedItemId;
            if (menuItemId != -1 && selectedId.HasValue &&  menuItemId != selectedId.Value)
            {
                _navigationBar?.Menu.FindItem(menuItemId)?.SetChecked(true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _navigationBar?.Dispose();
                _navigationService?.Dispose();
                _dependenciesContainer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}