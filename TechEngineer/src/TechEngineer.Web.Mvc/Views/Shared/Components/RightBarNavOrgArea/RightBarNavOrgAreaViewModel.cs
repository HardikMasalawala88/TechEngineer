using System.Collections.Generic;
using Abp.Localization;

namespace TechEngineer.Web.Views.Shared.Components.RightBarNavOrgArea
{
    public class RightBarNavOrgAreaViewModel
    {
        public LanguageInfo CurrentLanguage { get; set; }

        public IReadOnlyList<LanguageInfo> Languages { get; set; }
    }
}
