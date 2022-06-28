using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechEngineer.DBEntities.Locations.Dto
{
    /// <summary>
    /// Class to define paged location result request Dto.
    /// </summary>
    public class PagedLocationResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
        public Guid? OrganizationId { get; set; }
    }
}
