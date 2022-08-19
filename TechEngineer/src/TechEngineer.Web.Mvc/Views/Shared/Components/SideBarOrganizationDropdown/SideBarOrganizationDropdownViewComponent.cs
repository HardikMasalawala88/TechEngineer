using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TechEngineer.DBEntities.Organizations;
using TechEngineer.DBEntities.Organizations.Dto;

namespace TechEngineer.Web.Views.Shared.Components.SideBarOrganizationDropdown
{
    public class SideBarOrganizationDropdownViewComponent : TechEngineerViewComponent
    {
        private readonly IOrganizationAppService _organizationAppService;

        public SideBarOrganizationDropdownViewComponent(IOrganizationAppService organizationAppService)
        {
            _organizationAppService = organizationAppService;
        }

        public IViewComponentResult Invoke()
        {
            var organizations = _organizationAppService.GetOrganizationsAsync().Result;
            var model = new SideBarOrganizationListViewModel
            {
                Organizations = organizations.Items,
            };
            
            return View(model);
        }
    }
}
