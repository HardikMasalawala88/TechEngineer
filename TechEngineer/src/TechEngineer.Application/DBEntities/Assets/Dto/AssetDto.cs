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

        /// <summary>
        /// Gets or sets details.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets model number.
        /// </summary>
        public string ModelNumber { get; set; }

        /// <summary>
        /// Gets or sets department.
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets IPAddress.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets system username.
        /// </summary>
        public string System_Username { get; set; }

        /// <summary>
        /// Gets or sets cpu.
        /// </summary>
        public string CPU { get; set; }

        /// <summary>
        /// Gets or sets mother board.
        /// </summary>
        public string MotherBoard { get; set; }

        /// <summary>
        /// Gets or sets ram.
        /// </summary>
        public string RAM { get; set; }

        /// <summary>
        /// Gets or sets hdd.
        /// </summary>
        public string HDD { get; set; }

        /// <summary>
        /// Gets or sets monitor.
        /// </summary>
        public string Monitor { get; set; }

        /// <summary>
        /// Gets or sets monitor serial number.
        /// </summary>
        public string Monitor_SerialNo { get; set; }

        /// <summary>
        /// Gets or sets keyboard.
        /// </summary>
        public string KeyBoard { get; set; }

        /// <summary>
        /// Gets or sets mouse.
        /// </summary>
        public string Mouse { get; set; }

        /// <summary>
        /// Gets or sets opearting system.
        /// </summary>
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Gets or sets ms office.
        /// </summary>
        public string MSOffice { get; set; }

        /// <summary>
        /// Gets or sets purchase date.
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Gets or sets boolean value for warrenty.
        /// </summary>
        public bool IsInWarrenty { get; set; }

        /// <summary>
        /// Gets or sets is active boolean value.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets start warrenty date.
        /// </summary>
        public DateTime StartWarrentyDate { get; set; }

        /// <summary>
        /// Gets or sets end date of warrenty.
        /// </summary>
        public DateTime EndWarrentyDate { get; set; }
    }
}
