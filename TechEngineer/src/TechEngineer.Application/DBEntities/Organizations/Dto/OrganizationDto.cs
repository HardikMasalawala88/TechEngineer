using Abp.Application.Services.Dto;
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
    /// Class to define organization dto.
    /// </summary>
    [AutoMapFrom(typeof(OrganizationEntity))]
    public class OrganizationDto : EntityDto<Guid>
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets contact person name.
        /// </summary>
        [Required]
        public string ContactPersonName { get; set; }

        /// <summary>
        /// Gets or sets contact number.
        /// </summary>
        [Required]
        public string ContactNumber { get; set; }

        /// <summary>
        /// Gets or sets gst number.
        /// </summary>
        [Required]
        public string GSTNumber { get; set; }

        /// <summary>
        /// Gets or sets primary email address.
        /// </summary>
        [Required]
        public string PrimaryEmailAddress { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets address entity.
        /// </summary>
        public virtual LocationDto Location { get; set; }
    }

    /// <summary>
    /// Class to define paged organization result request dto.
    /// </summary>
    public class PagedOrganizationResultRequestDto : PagedResultRequestDto
    {
        /// <summary>
        /// Gets or sets keyword field.
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets a value inidicating whether.
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value inidicating organization.
        /// </summary>
        public Guid? OrganizationId { get; set; }
    }
}
