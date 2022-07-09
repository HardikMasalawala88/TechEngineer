using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Threading.Tasks;
using TechEngineer.DBEntities.Organizations.Dto;

namespace TechEngineer.DBEntities.Organizations
{
    /// <summary>
    /// Class to define organization app service.
    /// </summary>
    public interface IOrganizationAppService : IAsyncCrudAppService<OrganizationDto, Guid, PagedOrganizationResultRequestDto, CreateOrganizationDto, OrganizationDto>
    {
        /// <summary>
        /// Method to get list of organizations.
        /// </summary>
        /// <returns>Return list of organization.</returns>
        Task<ListResultDto<OrganizationDto>> GetOrganizationsAsync();
    }
}
