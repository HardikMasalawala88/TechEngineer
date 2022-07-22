using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechEngineer.DBEntities.Assets.Dto
{
    /// <summary>
    /// Add new asset information.
    /// </summary>
    [AutoMapTo(typeof(AssetEntity))]
    public class CreateAssetDto
    {
        /// <summary>
        /// Asset name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        [Required]
        public string Category { get; set; }//FK Category[Laptop / Desktop / CCTV / Biometrics / SoundSystem / internet...]

        /// <summary>
        /// Part unique number.
        /// </summary>
        [Required]
        public string SerialNumber { get; set; } = Guid.NewGuid().ToString();

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

        public string Details { get; set; }

        public string ModelNumber { get; set; }
        public bool IsInWarrenty { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartWarrentyDate { get; set; }
        public DateTime EndWarrentyDate { get; set; }

    }
}
