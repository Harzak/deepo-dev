using Android.Views;

namespace Deepo.Client.App
{
    public class HomeFragment : AndroidX.Fragment.App.Fragment
    {
        public override View? OnCreateView(LayoutInflater? inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater?.Inflate(Resource.Layout.layout_home, container, false);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
