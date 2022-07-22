using System.Collections.Generic;
using TechEngineer.DBEntities.Assets.Dto;
using TechEngineer.DBEntities.Locations.Dto;

namespace TechEngineer.Web.Models.Assets
{
    public class AssetListViewModel
    {
        public IReadOnlyList<AssetDto> Assets { get; set; }
        public IReadOnlyList<LocationDto> Locations { get; set; }
    }
}
