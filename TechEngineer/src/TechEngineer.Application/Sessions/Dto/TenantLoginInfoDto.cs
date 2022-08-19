using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using TechEngineer.MultiTenancy;

namespace TechEngineer.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
