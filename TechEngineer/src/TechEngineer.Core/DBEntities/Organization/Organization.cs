using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechEngineer.DBEntities.Organization
{
    [Table("Organizations")]
    public class OrganizationEntity : FullAuditedEntity<Guid>, IFullAudited
    {
        public OrganizationEntity()
        {
            CreationTime = Clock.Now;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ContactPersonName { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public string GSTNumber { get; set; }

        [Required]
        public string PrimaryEmailAddress { get; set; }

        public bool IsActive { get; set; }

    }
}
