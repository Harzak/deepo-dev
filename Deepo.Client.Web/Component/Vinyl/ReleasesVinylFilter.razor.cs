namespace Deepo.Client.Web.Component.Vinyl;

public partial class ReleasesVinylFilter
{
    private DateTime? _selectedDate;

    protected override void OnInitialized()
    {
        _selectedDate = DateTime.Now;
    }
}