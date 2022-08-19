using System.Collections.Generic;
using TechEngineer.DBEntities.Appointments.Dto;
using TechEngineer.DBEntities.Assets.Dto;
using TechEngineer.DBEntities.Locations.Dto;

namespace TechEngineer.Web.Models.Appointments
{
    public class AppointmentListViewModel
    {
        public IReadOnlyList<AppointmentDto> Appointments { get; set; }
        public IReadOnlyList<LocationDto> Locations { get; set; }
        public IReadOnlyList<AssetDto> Assets { get; set; }
    }
}
