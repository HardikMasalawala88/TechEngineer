using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechEngineer.DBEntities.Locations.Dto;
using TechEngineer.DBEntities.Organization;

namespace TechEngineer.DBEntities.Organizations.Dto
{
    /// <summary>
    /// Class to create organization dto.
    /// </summary>
    [AutoMapTo(typeof(OrganizationEntity))]
    public class CreateOrganizationDto
    {
        /// <summary>
        /// Gets or sets name field.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets contact person name.
        /// </summary>
        public string ContactPersonName { get; set; }

        /// <summary>
        /// Gets or sets contact number.
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// Gets or sets gst number.
        /// </summary>
        public string GSTNumber { get; set; }

        /// <summary>
        /// Gets or sets primary email address
        /// </summary>
        public string PrimaryEmailAddress { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether.
        /// </summary>
        public bool IsActive { get; set; }

        ///// <summary>
        ///// Gets or sets address id.
        ///// </summary>

        //public Guid LocationId { get; set; }

        /// <summary>
        /// Gets or sets address entity.
        /// </summary>
        public virtual CreateLocationDto Location { get; set; }
    }
}
