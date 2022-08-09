using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechEngineer.DBEntities.Location;
using TechEngineer.DBEntities.Organization;

namespace TechEngineer.DBEntities.Assets
{
    [Table("Assets")]
    public class AssetEntity : FullAuditedEntity<Guid>, IFullAudited
    {
        public AssetEntity()
        {
            CreationTime = Clock.Now;
        }

        [Required]
        public string Name { get; set; }

        public string Details { get; set; }

        [Required]
        public string Category { get; set; }//FK Category[Laptop / Desktop / CCTV / Biometrics / SoundSystem / internet...]

        [Required]
        public string SerialNumber { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string ModelNumber { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string IPAddress { get; set; }

        [Required]
        public string System_Username { get; set; }

        [Required]
        public string CPU { get; set; }

        [Required]
        public string MotherBoard { get; set; }

        [Required]
        public string RAM { get; set; }

        [Required]
        public string HDD { get; set; }

        [Required]
        public string Monitor { get; set; }

        [Required]
        public string Monitor_SerialNo { get; set; }

        [Required]
        public string KeyBoard { get; set; }

        [Required]
        public string Mouse { get; set; }

        [Required]
        public string OperatingSystem { get; set; }

        [Required]
        public string MSOffice { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsInWarrenty { get; set; }

        public DateTime StartWarrentyDate { get; set; }

        public DateTime EndWarrentyDate { get; set; }

        [Required]
        [ForeignKey("Organizations")]
        public Guid OrganizationId { get; set; }

        [Required]
        [ForeignKey("Locations")]
        public Guid LocationId { get; set; }

        public virtual OrganizationEntity Organization { get; set; }

        public virtual LocationEntity Location { get; set; }
    }
}
