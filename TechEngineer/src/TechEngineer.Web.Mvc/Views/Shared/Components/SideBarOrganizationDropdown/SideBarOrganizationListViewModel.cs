using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechEngineer.DBEntities.Organizations.Dto;

namespace TechEngineer.Web.Views.Shared.Components.SideBarOrganizationDropdown
{
    public class SideBarOrganizationListViewModel
    {
        public IReadOnlyList<OrganizationDto> Organizations { get; set; }
    }
}
