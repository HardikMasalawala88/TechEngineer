using System.Threading.Tasks;
using Abp.Application.Services;
using TechEngineer.Sessions.Dto;

namespace TechEngineer.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
