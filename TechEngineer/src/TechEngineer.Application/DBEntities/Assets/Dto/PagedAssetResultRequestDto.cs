using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechEngineer.DBEntities.Assets.Dto
{
    public class PagedAssetResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid? LocationId { get; set; }
    }
}
