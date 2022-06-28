using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using TechEngineer.Controllers;

namespace TechEngineer.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : TechEngineerControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
