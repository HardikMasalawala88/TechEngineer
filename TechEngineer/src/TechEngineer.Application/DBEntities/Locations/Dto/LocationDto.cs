using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechEngineer.DBEntities.Location;
using TechEngineer.DBEntities.Organizations.Dto;

namespace TechEngineer.DBEntities.Locations.Dto
{
    /// <summary>
    /// Class to define Locations dto.
    /// </summary>
    [AutoMapFrom(typeof(LocationEntity))]
    public class LocationDto : EntityDto<Guid>
    {
        /// <summary>
        /// Gets or sets address 1.
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets address 2.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets land mark.
        /// </summary>
        public string Landmark { get; set; }

        /// <summary>
        /// Gets or sets branch ITHead email address
        /// </summary>
        public string BranchITHeadEmail { get; set; }

        /// <summary>
        /// Gets or sets city id.
        /// </summary>
        public string CityId { get; set; }

        /// <summary>
        /// Gets or sets state id.
        /// </summary>
        public string StateId { get; set; }

        /// <summary>
        /// Gets or sets country id.
        /// </summary>
        public string CountryId { get; set; }

        /// <summary>
        /// Gets or sets postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether.
        /// </summary>
        public bool IsBaseLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets organization Id.
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// Gets organization object.
        /// </summary>
        public virtual OrganizationDto Organization { get; set; }

    }
}
