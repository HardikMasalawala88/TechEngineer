using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechEngineer.DBEntities.Appointments.Dto
{
    /// <summary>
    /// Class to define Appointment Map Profile.
    /// </summary>
    public class AppointmentMapProfile : Profile
    {
        /// <summary>
        /// Constructor for appointment map profile.
        /// </summary>
        public AppointmentMapProfile()
        {
            CreateMap<AppointmentDto, AppointmentEntity>();
            CreateMap<AppointmentDto, AppointmentEntity>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateAppointmentDto, AppointmentEntity>();
            CreateMap<CreateAppointmentDto, AppointmentEntity>();
        }
    }
}
