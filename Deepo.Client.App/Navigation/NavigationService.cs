using Android.Views;
using Deepo.Client.App.Fragments;
using Deepo.Client.App.Fragments.Factory;
using Deepo.Client.App.Navigation.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Client.App.Navigation
{
    public class NavigationService : INavigationService
    {
        private bool disposedValue;
        private readonly ILogger _logger;

        private readonly AndroidX.Fragment.App.FragmentManager _fragmentManager;
        private readonly IFragmentFactory _fragmentFactory;
        private readonly INavigationListener? _listener;
        private readonly Stack<int> _navigationHistory;

        public NavigationService(AndroidX.Fragment.App.FragmentManager fragmentManager,
            IFragmentFactory fragmentFactory,
            ILogger logger)
        {
            _logger = logger;
            _fragmentFactory = fragmentFactory;
            _fragmentManager = fragmentManager;
            _navigationHistory = new Stack<int>();
        }

        public NavigationService(AndroidX.Fragment.App.FragmentManager fragmentManager,
            INavigationListener listener,
            IFragmentFactory fragmentFactory,
            ILogger logger)
        : this(fragmentManager, fragmentFactory, logger)
        {
            _listener = listener;
            _navigationHistory = new Stack<int>();
        }

        public bool NavigateTo(IMenuItem item)
            => item?.ItemId switch
            {
                Resource.Id.navigate_to_home => NavigateToHome(),
                Resource.Id.navigate_to_newReleases => NavigateToNewReleases(),
                Resource.Id.navigate_to_settings => NavigateToSettings(),
                _ => false
            };

        public bool NavigateToHome()
        {
            return NavigateTo(new HomeFragment(), Resource.Id.layout_home);
        }

        public bool NavigateToNewReleases()
        {
            NewReleasesFragment fragment = _fragmentFactory.CreateNewReleasesFragment();
            return NavigateTo(fragment, Resource.Id.layout_newReleases);
        }

        public bool NavigateToSettings()
        {
            return NavigateTo(new SettingsFragment(), Resource.Id.layout_settings);
        }

        public bool NavigateBack(Activity activity)
        {
            ArgumentNullException.ThrowIfNull(activity);

            if (_fragmentManager.BackStackEntryCount == 0)
            {
                activity.Finish();
            }
            else
            {
                _fragmentManager.PopBackStackImmediate();
                _navigationHistory.Pop();
                _listener?.OnNavigatedBack(GetActiveLayoutId());
            }
            return false;
        }

        private bool NavigateTo(AndroidX.Fragment.App.Fragment fragment, int layoutId)
        {
            if (GetActiveLayoutId() != layoutId && StartFragment(fragment))
            {
                _navigationHistory.Push(layoutId);
                _listener?.OnNavigated(GetActiveLayoutId());
                return true;
            }
            return false;
        }

        private bool StartFragment(AndroidX.Fragment.App.Fragment fragment)
        {
            return _fragmentManager.BeginTransaction()
                              .Replace(Resource.Id.content_frame, fragment)
                              .AddToBackStack(fragment.ToString())
                              .Commit() > -1;
        }

        private int GetActiveLayoutId()
        {
            if (!_navigationHistory.TryPeek(out int result))
            {
                return -1;
            }
            return result;
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _fragmentManager?.Dispose();
                }
                _navigationHistory?.Clear();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~NavigationService()
          => Dispose(disposing: false);
        #endregion
    }
}
