namespace Deepo.Client.Web.Component.Vinyl;

public partial class Filter
{
    private DateTime? _selectedDate;

    protected override void OnInitialized()
    {
        _selectedDate = DateTime.Now;
    }
}