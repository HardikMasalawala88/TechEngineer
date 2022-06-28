using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechEngineer.DBEntities.Locations.Dto;

namespace TechEngineer.DBEntities.Locations
{
    /// <summary>
    /// Class to define location app service.
    /// </summary>
    public interface ILocationAppService : IAsyncCrudAppService<LocationDto, Guid, PagedLocationResultRequestDto, CreateLocationDto, LocationDto>
    {
        /// <summary>
        /// Method to get base location by using organization.
        /// </summary>
        /// <param name="input">Input parameter.</param>
        /// <returns>Return location data.</returns>
        Task<LocationDto> GetBaseLocationByOrganizationAsync(EntityDto<Guid> input);
    }
}
