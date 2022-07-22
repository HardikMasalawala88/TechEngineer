using System.Collections.Generic;
using TechEngineer.DBEntities.Locations.Dto;

namespace TechEngineer.Web.Models.Locations
{
    public class LocationListViewModel
    {
        public IReadOnlyList<LocationDto> Locations { get; set; }
    }
}
