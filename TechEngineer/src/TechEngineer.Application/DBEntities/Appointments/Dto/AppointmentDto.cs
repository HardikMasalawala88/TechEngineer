using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using TechEngineer.Authorization.Users;
using TechEngineer.DBEntities.Assets.Dto;
using TechEngineer.DBEntities.Locations.Dto;
using TechEngineer.DBEntities.Organizations.Dto;

namespace TechEngineer.DBEntities.Appointments.Dto
{
    /// <summary>
    /// Class to define appointment dto.
    /// </summary>
    [AutoMapFrom(typeof(AppointmentEntity))]
    public class AppointmentDto : EntityDto<Guid>
    {
        /// <summary>
        /// Gets or sets requested date.
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Gets or sets appointment status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets appointment remarks.
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets asset id.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets location id.
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Gets or sets organization Id.
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// Gets organization object.
        /// </summary>
        public virtual OrganizationDto Organization { get; set; }

        /// <summary>
        /// Gets location object.
        /// </summary>
        public virtual LocationDto Location { get; set; }

        /// <summary>
        /// Gets location object.
        /// </summary>
        public virtual AssetDto Asset { get; set; }

        /// <summary>
        /// Gets location object.
        /// </summary>
        public virtual User User { get; set; }
    }
}
