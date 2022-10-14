using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
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

        //public async Task<ActionResult> ImportAssetsData(IFormCollection file)
        //{
        //    try
        //    {
        //        var filename = ContentDispositionHeaderValue.Parse(file.Files[0].ContentDisposition).FileName.Trim('"');

        //        //get path
        //        var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

        //        //create directory "Uploads" if it doesn't exists
        //        if (!Directory.Exists(MainPath))
        //        {
        //            Directory.CreateDirectory(MainPath);
        //        }

        //        //get file path 
        //        var filePath = Path.Combine(MainPath, filename);
        //        using (Stream stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await file.Files[0].CopyToAsync(stream);
        //        }

        //        //get extension
        //        string extension = Path.GetExtension(filename);
        //        string conString = string.Empty;

        //        switch (extension)
        //        {
        //            case ".xls": //Excel 97-03.
        //                conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
        //                break;
        //            case ".xlsx": //Excel 07 and above.
        //                conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
        //                break;
        //        }

        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("Id",typeof(Guid));
        //        conString = string.Format(conString, filePath);

        //        using (OleDbConnection connExcel = new OleDbConnection(conString))
        //        {
        //            using (OleDbCommand cmdExcel = new OleDbCommand())
        //            {
        //                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
        //                {
        //                    cmdExcel.Connection = connExcel;

        //                    //Get the name of First Sheet.
        //                    connExcel.Open();
        //                    DataTable dtExcelSchema;
        //                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        //                    connExcel.Close();

        //                    //Read Data from First Sheet.
        //                    connExcel.Open();
        //                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
        //                    odaExcel.SelectCommand = cmdExcel;
        //                    odaExcel.Fill(dt);
        //                    connExcel.Close();
        //                }
        //            }
        //        }

        //        //your database connection string
        //        conString = @"Server=techengineerdb.database.windows.net;Database=TechEngineerDb;User ID=TechEngineer;Password=Demo@124;";

        //        using (SqlConnection con = new SqlConnection(conString))
        //        {
        //            try
        //            {
        //                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
        //                {
        //                    //Set the database table name.
        //                    sqlBulkCopy.DestinationTableName = "dbo.Assets";

        //                    // Map the Excel columns with that of the database table, this is optional but good if you do
        //                    // 
        //                    object Id = new Guid();
        //                    Id = Guid.NewGuid();
        //                    //dt.Rows;
        //                    dt.Rows[0].ItemArray.SetValue(Id, 0);
        //                    con.Open();
        //                    sqlBulkCopy.WriteToServer(dt);
        //                    con.Close();
        //                }
        //            }
        //            catch (Exception ecx)
        //            {

                        
        //            }
        //        }
        //        return Json(new { Status = 1, Message = "File Imported Successfully " });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Status = 0, Message = ex.Message });
        //    }
        //}
    }
}
