using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechEngineer.Authorization.Users;
using TechEngineer.DBEntities.Assets;
using TechEngineer.DBEntities.Location;
using TechEngineer.DBEntities.Organization;

namespace TechEngineer.DBEntities.Appointments
{
    [Table("Appointments")]
    public class AppointmentEntity : FullAuditedEntity<Guid>, IFullAudited
    {
        public AppointmentEntity()
        {
            CreationTime = Clock.Now;
        }

        public DateTime RequestDate { get; set; }

        public string Status { get; set; }

        [Required]
        [ForeignKey("Users")]
        public long UserId { get; set; }

        [Required]
        [ForeignKey("Assets")]
        public Guid AssetId { get; set; }

        [Required]
        [ForeignKey("Organizations")]
        public Guid OrganizationId { get; set; }

        [Required]
        [ForeignKey("Locations")]
        public Guid LocationId { get; set; }

        public virtual User Users { get; set; }

        public virtual AssetEntity Asset { get; set; }

        public virtual OrganizationEntity Organization { get; set; }

        public virtual LocationEntity Location { get; set; }
    }
}
