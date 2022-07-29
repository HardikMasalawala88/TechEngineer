using TechEngineer.DBEntities.Appointments.Dto;
using TechEngineer.DBEntities.Assets.Dto;
using TechEngineer.DBEntities.Locations.Dto;

namespace TechEngineer.Web.Blazor.Models.Appointments
{
    public class EditAppointmentViewModel
    {
        public AppointmentDto Appointment { get; set; }
        public AssetDto Asset { get; set; }
        public LocationDto Location { get; set; }
    }
}
