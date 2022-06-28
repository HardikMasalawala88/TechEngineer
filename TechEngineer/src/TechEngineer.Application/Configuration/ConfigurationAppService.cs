using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using TechEngineer.Configuration.Dto;

namespace TechEngineer.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : TechEngineerAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
