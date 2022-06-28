using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace TechEngineer.Web.Views
{
    public abstract class TechEngineerRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected TechEngineerRazorPage()
        {
            LocalizationSourceName = TechEngineerConsts.LocalizationSourceName;
        }
    }
}
