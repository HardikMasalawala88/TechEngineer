using System.Linq;
using Abp.Localization;
using Microsoft.AspNetCore.Mvc;

namespace TechEngineer.Web.Views.Shared.Components.RightBarNavOrgArea
{
    public class RightBarNavOrgAreaViewComponent : TechEngineerViewComponent
    {
        private readonly ILanguageManager _languageManager;

        public RightBarNavOrgAreaViewComponent(ILanguageManager languageManager)
        {
            _languageManager = languageManager;
        }

        public IViewComponentResult Invoke()
        {
            var model = new RightBarNavOrgAreaViewModel
            {
                CurrentLanguage = _languageManager.CurrentLanguage,
                Languages = _languageManager.GetLanguages().Where(l => !l.IsDisabled).ToList()
            };

            return View(model);
        }
    }
}
