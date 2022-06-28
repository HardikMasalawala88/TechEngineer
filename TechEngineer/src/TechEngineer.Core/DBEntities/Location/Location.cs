using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechEngineer.DBEntities.Organization;

namespace TechEngineer.DBEntities.Location
{
    [Table("Locations")]
    public class LocationEntity : FullAuditedEntity<Guid>, IFullAudited
    {
        public LocationEntity()
        {
            CreationTime = Clock.Now;
        }

        [Required]
        public string Address1 { get; set; }

        [Required]
        public string Address2 { get; set; }

        [Required]
        public string Landmark { get; set; }

        [Required]
        public string CityId { get; set; }

        [Required]
        public string StateId { get; set; }

        [Required]
        public string CountryId { get; set; }

        [Required]
        public string PostalCode { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public bool IsBaseLocation { get; set; }

        [Required]
        [ForeignKey("Organizations")]
        public Guid OrganizationId { get; set; }

        public virtual OrganizationEntity Organization { get; set; }
    }
}
