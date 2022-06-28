using Abp.AspNetCore.Mvc.ViewComponents;

namespace TechEngineer.Web.Views
{
    public abstract class TechEngineerViewComponent : AbpViewComponent
    {
        protected TechEngineerViewComponent()
        {
            LocalizationSourceName = TechEngineerConsts.LocalizationSourceName;
        }
    }
}
