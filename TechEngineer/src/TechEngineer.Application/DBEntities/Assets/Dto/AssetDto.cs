using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechEngineer.DBEntities.Assets.Dto
{
    /// <summary>
    /// Class to define asset dto.
    /// </summary>
    [AutoMapFrom(typeof(AssetEntity))]
    public class AssetDto : EntityDto<Guid>
    {
        /// <summary>
        /// Gets or sets name field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets category field.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets serial number.
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets location id.
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Gets or sets organization Id.
        /// </summary>
        public Guid OrganizationId { get; set; }

        public string Details { get; set; }
        public string ModelNumber { get; set; }
        public bool IsInWarrenty { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartWarrentyDate { get; set; }
        public DateTime EndWarrentyDate { get; set; }
    }
}
