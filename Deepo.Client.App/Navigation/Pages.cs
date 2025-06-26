namespace Deepo.Client.App.Navigation
{
    public static class Pages
    {
        private static readonly Dictionary<int, int> _catalog = new Dictionary<int, int>()
        {
            { Resource.Id.navigate_to_home, Resource.Id.layout_home },
            { Resource.Id.navigate_to_newReleases, Resource.Id.layout_newReleases },
            { Resource.Id.navigate_to_settings, Resource.Id.layout_settings}
        };

        public static Dictionary<int, int> Catalog
        {
            get => _catalog;
        }
    }
}
