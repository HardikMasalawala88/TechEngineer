using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechEngineer.DBEntities.Appointments.Dto
{
    /// <summary>
    /// Class to define Paged Appointment Result Request Dto.
    /// </summary>
    public class PagedAppointmentResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? UserId { get; set; }
        public Guid? AssetId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid? LocationId { get; set; }
    }
}
