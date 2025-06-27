using Deepo.Client.Web.Dto;

namespace Deepo.Client.Web.Interfaces;

public interface IVinylCatalog : ICatalog
{
    IFilteredCollection<ReleaseVinylDto> Items { get; }
}