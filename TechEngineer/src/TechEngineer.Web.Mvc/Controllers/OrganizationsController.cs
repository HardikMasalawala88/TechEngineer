using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechEngineer.Authorization;
using TechEngineer.DBEntities.Organizations;
using TechEngineer.Web.Models.Organizations;

namespace TechEngineer.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Organizations)]
    public class OrganizationsController : Controller
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
    }
}
