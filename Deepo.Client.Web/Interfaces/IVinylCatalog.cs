using Deepo.Client.Web.Dto;

namespace Deepo.Client.Web.Interfaces;

/// <summary>
/// Defines the contract for a catalog specifically designed for vinyl releases.
/// </summary>
public interface IVinylCatalog : ICatalog
{
    /// <summary>
    /// Gets the filtered collection of vinyl release items.
    /// </summary>
    IFilteredCollection<ReleaseVinylDto> Items { get; }
}