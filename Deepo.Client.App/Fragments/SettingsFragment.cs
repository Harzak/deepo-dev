using Android.Views;

namespace Deepo.Client.App.Fragments
{
    public class SettingsFragment : AndroidX.Fragment.App.Fragment
    {
        public override View? OnCreateView(LayoutInflater? inflater, ViewGroup? container, Bundle? savedInstanceState)
        {

            return inflater?.Inflate(Resource.Layout.layout_settings, container, false);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
