using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechEngineer.Authorization;
using TechEngineer.Controllers;
using TechEngineer.DBEntities.Appointments;
using TechEngineer.DBEntities.Assets;
using TechEngineer.DBEntities.Assets.Dto;
using TechEngineer.DBEntities.Locations;
using TechEngineer.DBEntities.Organizations;
using TechEngineer.Web.Models.Appointments;

namespace TechEngineer.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Appointments)]
    public class AppointmentsController : TechEngineerControllerBase
    {
        private readonly IAppointmentAppService _appointmentAppService;
        private readonly IAssetAppService _assetAppService;
        private readonly ILocationAppService _locationAppService;
        private readonly IOrganizationAppService _orgAppService;

        public AppointmentsController(IAppointmentAppService appointmentAppService, IAssetAppService assetAppService,
                ILocationAppService locationAppService, IOrganizationAppService orgAppService)
        {
            _appointmentAppService = appointmentAppService;
            _assetAppService = assetAppService;
            _locationAppService = locationAppService;
            _orgAppService = orgAppService;
        }

        public IActionResult Index()
        {
            var appointments = _appointmentAppService.GetAppointmentsAsync().Result.Items;
            var locations = _locationAppService.GetLocationsAsync().Result.Items;
            var assets = _assetAppService.GetAssetsAsync().Result.Items;

            var model = new AppointmentListViewModel
            {
                Appointments = appointments,
                Assets = assets,
                Locations = locations
            };

            return View(model);
        }

        public async Task<ActionResult> EditModal(Guid appointmentId)
        {
            var output = await _appointmentAppService.GetAppointmentForEdit(new EntityDto<Guid>(appointmentId));
            var asset = _assetAppService.GetAssetForEdit(new EntityDto<Guid>(output.AssetId));
            var model = new EditAppointmentViewModel
            {
                Appointment = output,
                Asset = await asset
            };

            return PartialView("_EditModal", model);
        }
    }
}
