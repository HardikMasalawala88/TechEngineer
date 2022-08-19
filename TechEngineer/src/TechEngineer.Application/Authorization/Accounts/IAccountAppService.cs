using System.Threading.Tasks;
using Abp.Application.Services;
using TechEngineer.Authorization.Accounts.Dto;

namespace TechEngineer.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
