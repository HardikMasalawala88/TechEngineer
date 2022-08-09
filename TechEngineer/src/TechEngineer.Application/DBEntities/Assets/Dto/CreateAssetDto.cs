using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;

namespace TechEngineer.DBEntities.Assets.Dto
{
    /// <summary>
    /// Add new asset information.
    /// </summary>
    [AutoMapTo(typeof(AssetEntity))]
    public class CreateAssetDto
    {
        /// <summary>
        /// Gets or sets Asset name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Category name.
        /// </summary>
        [Required]
        public string Category { get; set; }//FK Category[Laptop / Desktop / CCTV / Biometrics / SoundSystem / internet...]

        /// <summary>
        /// Gets or sets Part unique number.
        /// </summary>
        [Required]
        public string SerialNumber { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets Selected Organization id.
        /// </summary>
        [Required]
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets Selected Location id of Organization id.
        /// </summary>
        [Required]
        public Guid LocationId { get; set; }

        /// <summary>
        /// Gets or sets department.
        /// </summary>
        [Required]
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets IPAddress.
        /// </summary>
        [Required]
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets System user name.
        /// </summary>
        [Required]
        public string SystemUsername { get; set; }

        /// <summary>
        /// Gets or sets CPU.
        /// </summary>
        [Required]
        public string CPU { get; set; }

        /// <summary>
        /// Gets or sets MotherBoard.
        /// </summary>
        [Required]
        public string MotherBoard { get; set; }

        /// <summary>
        /// Gets or sets RAM.
        /// </summary>
        [Required]
        public string RAM { get; set; }

        /// <summary>
        /// Gets or sets HDD.
        /// </summary>
        [Required]
        public string HDD { get; set; }

        /// <summary>
        /// Gets or sets Monitor.
        /// </summary>
        [Required]
        public string Monitor { get; set; }

        /// <summary>
        /// Gets or sets Monitor_SerialNo.
        /// </summary>
        [Required]
        public string Monitor_SerialNo { get; set; }

        /// <summary>
        /// Gets or sets KeyBoard.
        /// </summary>
        [Required]
        public string KeyBoard { get; set; }

        /// <summary>
        /// Gets or sets Mouse.
        /// </summary>
        [Required]
        public string Mouse { get; set; }

        /// <summary>
        /// Gets or sets OperatingSystem.
        /// </summary>
        [Required]
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Gets or sets MSOffice.
        /// </summary>
        [Required]
        public string MSOffice { get; set; }

        /// <summary>
        /// Gets or sets PurchaseDate.
        /// </summary>
        [Required]
        public DateTime PurchaseDate { get; set; }

        public string Details { get; set; }

        public string ModelNumber { get; set; }
        public bool IsInWarrenty { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartWarrentyDate { get; set; }
        public DateTime EndWarrentyDate { get; set; }

    }
}
