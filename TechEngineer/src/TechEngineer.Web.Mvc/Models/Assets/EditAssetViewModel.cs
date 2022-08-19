using TechEngineer.DBEntities.Assets.Dto;
using TechEngineer.DBEntities.Locations.Dto;

namespace TechEngineer.Web.Models.Assets
{
    public class EditAssetViewModel
    {
        public AssetDto Asset { get; set; }
        public LocationDto Location { get; set; }
    }
}
