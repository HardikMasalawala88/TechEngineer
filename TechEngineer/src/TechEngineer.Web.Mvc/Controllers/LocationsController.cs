using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TechEngineer.Authorization;
using TechEngineer.Controllers;
using TechEngineer.DBEntities.Locations;
using TechEngineer.Web.Models.Locations;

namespace TechEngineer.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Organizations)]
    public class LocationsController : TechEngineerControllerBase
    {
        private readonly ILocationAppService _locationAppService;

        public LocationsController(ILocationAppService locationAppService)
        {
            _locationAppService = locationAppService;
        }

        public IActionResult Index()
        {
            var locations = _locationAppService.GetLocationsAsync().Result;
            var model = new LocationListViewModel
            {
                Locations = locations.Items
            };

            return View(model);
        }

        public async Task<ActionResult> EditModal(Guid locationId)
        {
            var output = await _locationAppService.GetLocationForEdit(new EntityDto<Guid>(locationId));
            var model = new EditLocationViewModel
            {
                Locations = output
            };

            return PartialView("_EditModal", model);
        }
    }
}
