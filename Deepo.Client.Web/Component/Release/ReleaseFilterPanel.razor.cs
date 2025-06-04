using Deepo.Client.Web.Model;
using Deepo.Client.Web.Navigation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Deepo.Client.Web.Component.Release;

public partial class ReleaseFilterPanel
{
    private readonly List<ReleaseTabModel> _tabs = [];
    private EReleaseType _selectedRelease;
    private int ActivetabIndex
    {
        get => _tabs.Any(x => x.Type == _selectedRelease) ? _tabs.IndexOf(_tabs.First(x => x.Type == _selectedRelease)) : 0;
        set
        {
            _selectedRelease = _tabs[value].Type;
            SelectedReleaseChanged.InvokeAsync(_selectedRelease);
        }
    }

    protected override void OnInitialized()
    {
        _tabs.Add(new ReleaseTabModel()
        {
            Label = "Vinyl",
            ToolTip = "Last vinyl releases",
            Type = EReleaseType.Vinyl,
            Disabled = false,
            Icon = Icons.Material.Outlined.MusicNote
        });
        _tabs.Add(new ReleaseTabModel()
        {
            Label = "Movie",
            ToolTip = "Coming soon",
            Type = EReleaseType.Movie,
            Disabled = true,
            Icon = Icons.Material.Outlined.Movie
        });
        _tabs.Add(new ReleaseTabModel()
        {
            Label = "TV Show",
            ToolTip = "Coming soon",
            Type = EReleaseType.TvShow,
            Disabled = true,
            Icon = Icons.Material.Outlined.Tv
        });
    }

    [Parameter]
    public EReleaseType SelectedRelease
    {
        get => _selectedRelease;
        set
        {
            if (value != _selectedRelease)
            {
                _selectedRelease = value;
                SelectedReleaseChanged.InvokeAsync(value);
            }
        }
    }

    [Parameter]
    public EventCallback<EReleaseType> SelectedReleaseChanged { get; set; }
}

