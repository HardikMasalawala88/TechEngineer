using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TechEngineer.Authorization;
using TechEngineer.Controllers;
using TechEngineer.DBEntities.Assets;
using TechEngineer.DBEntities.Assets.Dto;
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

        public async Task<ActionResult> EditModal(Guid assetId)
        {
            var output = await _assetAppService.GetAssetForEdit(new EntityDto<Guid>(assetId));
            var model = new EditAssetViewModel
            {
                Asset = output,
                Location = _locationAppService.GetLocationById(output.LocationId)
            };

            return PartialView("_EditModal", model);
        }

        public ActionResult FillLocation(Guid orgId)
        {
            var locations = _locationAppService.GetLocationUsingOrgId(orgId);

            return Json(locations, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.None });
        }

        //public async Task<ActionResult> ImportFile(HttpPostedFileBase importFile)
        //{
        //    if (importFile == null) return Json(new { Status = 0, Message = "No File Selected" });

        //    try
        //    {
        //        var fileData = GetDataFromCSVFile(importFile.InputStream);

        //        var dtEmployee = fileData.ToDataTable();
        //        var tblEmployeeParameter = new SqlParameter("tblEmployeeTableType", SqlDbType.Structured)
        //        {
        //            TypeName = "dbo.tblTypeEmployee",
        //            Value = dtEmployee
        //        };
        //        await _dbContext.Database.ExecuteSqlCommandAsync("EXEC spBulkImportEmployee @tblEmployeeTableType", tblEmployeeParameter);
        //        return Json(new { Status = 1, Message = "File Imported Successfully " });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Status = 0, Message = ex.Message });
        //    }
        //}

        private List<AssetDto> GetDataFromCSVFile(Stream stream)
        {
            var assetList = new List<AssetDto>();
            try
            {
                using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true // To set First Row As Column Names    
                        }
                    });

                    if (dataSet.Tables.Count > 0)
                    {
                        var dataTable = dataSet.Tables[0];
                        foreach (DataRow objDataRow in dataTable.Rows)
                        {
                            if (objDataRow.ItemArray.All(x => string.IsNullOrEmpty(x?.ToString()))) continue;
                            assetList.Add(new AssetDto()
                            {
                                Id = (Guid)objDataRow["ID"],
                                Name = objDataRow["Name"].ToString(),
                                System_Username = objDataRow["System_Username"].ToString(),
                                Category = objDataRow["Category"].ToString(),
                                ModelNumber = objDataRow["ModelNumber"].ToString(),
                                Monitor = objDataRow["Monitor"].ToString(),
                                SerialNumber = objDataRow["SerialNumber"].ToString(),
                                Department = objDataRow["Department"].ToString(),
                                IPAddress = objDataRow["IPAddress"].ToString(),
                                CPU = objDataRow["CPU"].ToString(),
                                RAM = objDataRow["RAM"].ToString(),
                                MotherBoard = objDataRow["MotherBoard"].ToString(),
                                Mouse = objDataRow["Mouse"].ToString(),
                                Monitor_SerialNo = objDataRow["Monitor_SerialNo"].ToString(),
                                HDD = objDataRow["HDD"].ToString(),
                                KeyBoard = objDataRow["KeyBoard"].ToString(),
                                OperatingSystem = objDataRow["OperatingSystem"].ToString(),
                                MSOffice = objDataRow["MSOffice"].ToString(),
                                Details = objDataRow["Details"].ToString(),
                                IsInWarrenty = (bool)objDataRow["IsInWarrenty"],
                                IsActive = (bool)objDataRow["IsActive"],
                                PurchaseDate = (DateTime)objDataRow["PurchaseDate"],
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return assetList;
        }
    }
}
