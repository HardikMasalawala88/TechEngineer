using System.Collections.Generic;
using TechEngineer.DBEntities.Organizations.Dto;

namespace TechEngineer.Web.Models.Organizations
{
    public class OrganizationListViewModel
    {
        public IReadOnlyList<OrganizationDto> Organizations { get; set; }
    }
}
