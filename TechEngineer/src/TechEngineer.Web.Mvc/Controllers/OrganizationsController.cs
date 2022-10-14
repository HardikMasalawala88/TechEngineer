using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TechEngineer.Authorization;
using TechEngineer.Controllers;
using TechEngineer.DBEntities.Organizations;
using TechEngineer.DBEntities.Organizations.Dto;
using TechEngineer.Web.Models.Organizations;

namespace TechEngineer.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Organizations)]
    public class OrganizationsController : TechEngineerControllerBase
    {
        private readonly IOrganizationAppService _organizationAppService;

        public OrganizationsController(IOrganizationAppService organizationAppService)
        {
            _organizationAppService = organizationAppService;
        }

        public async Task<IActionResult> Index()
        {
            var organizations = _organizationAppService.GetOrganizationsAsync().Result;
            var model = new OrganizationListViewModel
            {
                Organizations = organizations.Items
            };

            return View(model);
        }

        public async Task<ActionResult> EditModal(Guid organizationId)
        {
            var output = await _organizationAppService.GetOrganizationForEdit(new EntityDto<Guid>(organizationId));
            var model = new EditOrgModalViewModel
            {
                Organization = (OrganizationDto)output,
                Locations = output.Location
            };

            return PartialView("_EditModal", model);
        }
    }
}
