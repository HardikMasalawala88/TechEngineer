using Abp.Application.Services;
using TechEngineer.MultiTenancy.Dto;

namespace TechEngineer.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

