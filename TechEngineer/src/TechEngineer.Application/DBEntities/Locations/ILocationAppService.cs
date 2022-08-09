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

        /// <summary>
        /// Method to get list of locations.
        /// </summary>
        /// <returns>Return list of location.</returns>
        Task<ListResultDto<LocationDto>> GetLocationsAsync();

        /// <summary>
        /// Method to get location for edit.
        /// </summary>
        /// <param name="input">Input parameter.</param>
        /// <returns>Return location data.</returns>
        Task<LocationDto> GetLocationForEdit(EntityDto<Guid> input);

        /// <summary>
        /// Get location by id.
        /// </summary>
        /// <param name="locationId">Location id.</param>
        /// <returns>Return location data.</returns>
        LocationDto GetLocationById(Guid locationId);

        /// <summary>
        /// Method to get locations from organization id.
        /// </summary>
        /// <param name="orgId">Organization id.</param>
        /// <returns>Returns list of location.</returns>
        List<LocationDto> GetLocationUsingOrgId(Guid orgId);
    }
}
