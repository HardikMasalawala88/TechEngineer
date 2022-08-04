using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechEngineer.DBEntities.Location;

namespace TechEngineer.DBEntities.Locations.Dto
{
    /// <summary>
    /// Class to create addresses dto.
    /// </summary>
    [AutoMapTo(typeof(LocationEntity))]
    public class CreateLocationDto
    {
        /// <summary>
        /// Gets or sets address 1.
        /// </summary>
        [Required]
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets Address 2.
        /// </summary>
        [Required]
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets Landmark.
        /// </summary>
        public string Landmark { get; set; }

        /// <summary>
        /// Gets or sets branch ITHead email address
        /// </summary>
        public string BranchITHeadEmail { get; set; }

        /// <summary>
        /// Gets or sets CityId.
        /// </summary>
        public string CityId { get; set; }

        /// <summary>
        /// Gets or sets StateId.
        /// </summary>
        public string StateId { get; set; }

        /// <summary>
        /// Gets or sets CountryId.
        /// </summary>
        public string CountryId { get; set; }

        /// <summary>
        /// Gets or sets PostalCode.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets IsBaseLocation.
        /// </summary>
        public bool IsBaseLocation { get; set; }

        /// <summary>
        /// Gets or sets IsActive.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets organization Id.
        /// </summary>
        public Guid OrganizationId { get; set; }
    }
}
