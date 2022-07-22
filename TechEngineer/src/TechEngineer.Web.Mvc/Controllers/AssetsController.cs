﻿using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechEngineer.Authorization;
using TechEngineer.Controllers;
using TechEngineer.DBEntities.Assets;
using TechEngineer.DBEntities.Locations;
using TechEngineer.Web.Models.Assets;

namespace TechEngineer.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Assets)]
    public class AssetsController : TechEngineerControllerBase
    {
        private readonly IAssetAppService _assetAppService;
        private readonly ILocationAppService _locationAppService;

        public AssetsController(IAssetAppService assetAppService, ILocationAppService locationAppService)
        {
            _assetAppService = assetAppService;
            _locationAppService = locationAppService;
        }

        public IActionResult Index()
        {
            var assets = _assetAppService.GetAssetsAsync().Result.Items;
            var locations = _locationAppService.GetLocationsAsync().Result.Items;
            var model = new AssetListViewModel
            {
                Assets = assets,
                Locations = locations
            };

            return View(model);
        }
    }
}