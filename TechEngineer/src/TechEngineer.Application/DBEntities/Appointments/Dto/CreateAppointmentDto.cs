using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechEngineer.DBEntities.Appointments.Dto
{
    /// <summary>
    /// Class to define create appointment dto.
    /// </summary>
    [AutoMapTo(typeof(AppointmentEntity))]
    public class CreateAppointmentDto
    {
        /// <summary>
        /// Gets or sets request date.
        /// </summary>
        [Required]
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Gets or sets status.
        /// </summary>
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets remarks.
        /// </summary>
        [Required]
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// Selected asset id.
        /// </summary>
        [Required]
        public Guid AssetId { get; set; }

        /// <summary>
        /// Selected Organization id.
        /// </summary>
        [Required]
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// Selected Location id of Organization id.
        /// </summary>
        [Required]
        public Guid LocationId { get; set; }
    }
}
