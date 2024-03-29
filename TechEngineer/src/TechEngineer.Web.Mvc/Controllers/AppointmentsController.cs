﻿using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TechEngineer.Authorization;
using TechEngineer.Controllers;
using TechEngineer.DBEntities.Appointments;
using TechEngineer.DBEntities.Assets;
using TechEngineer.DBEntities.Locations;
using TechEngineer.Web.Models.Appointments;

namespace TechEngineer.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Appointments)]
    public class AppointmentsController : TechEngineerControllerBase
    {
        private readonly IAppointmentAppService _appointmentAppService;
        private readonly IAssetAppService _assetAppService;
        private readonly ILocationAppService _locationAppService;

        public AppointmentsController(IAppointmentAppService appointmentAppService, IAssetAppService assetAppService,
                ILocationAppService locationAppService)
        {
            _appointmentAppService = appointmentAppService;
            _assetAppService = assetAppService;
            _locationAppService = locationAppService;
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
            var asset = _assetAppService.GetAssetForEdit(new EntityDto<Guid>(output.AssetId)).Result;
            var location = _locationAppService.GetLocationForEdit(new EntityDto<Guid>(asset.LocationId)).Result;
            var model = new EditAppointmentViewModel
            {
                Appointment = output,
                Asset = asset,
                Location = location
            };

            return PartialView("_EditModal", model);
        }

        public ActionResult FillLocation(Guid orgId)
        {
            var locations = _locationAppService.GetLocationUsingOrgId(orgId);

            return Json(locations, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.None });
        }

        public ActionResult FillAsset(Guid locationId)
        {
            var assets = _assetAppService.GetAssetsUsingLocationId(locationId);

            return Json(assets, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.None });
        }
    }
}
