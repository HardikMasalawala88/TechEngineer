using System.Threading.Tasks;
using TechEngineer.Configuration.Dto;

namespace TechEngineer.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
