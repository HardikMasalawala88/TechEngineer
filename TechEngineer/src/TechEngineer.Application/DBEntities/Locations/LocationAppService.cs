﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechEngineer.Authorization;
using TechEngineer.Authorization.Roles;
using TechEngineer.Authorization.Users;
using TechEngineer.DBEntities.Assets.Dto;
using TechEngineer.DBEntities.Location;
using TechEngineer.DBEntities.Locations.Dto;
using TechEngineer.DBEntities.Organization;

namespace TechEngineer.DBEntities.Locations
{
    /// <summary>
    /// Class to define Location app service.
    /// </summary>
    //[AbpAuthorize(PermissionNames.Pages_Locations)]
    public class LocationAppService : AsyncCrudAppService<LocationEntity, LocationDto, Guid, PagedLocationResultRequestDto, CreateLocationDto, LocationDto>, ILocationAppService
    {
        private readonly IRepository<LocationEntity, Guid> _locationRepository;
        private readonly IRepository<OrganizationEntity, Guid> _organizationRepository;
        private readonly IObjectMapper _objectMapper;
        private readonly UserManager _userManager;
        private readonly IAbpSession _abpSession;

        /// <summary>
        /// Constructor for Location service.
        /// </summary>
        /// <param name="repository">Repository for Location entity.</param>
        /// <param name="objectMapper">Object mapper.</param>
        public LocationAppService(IRepository<LocationEntity, Guid> repository,
            IRepository<OrganizationEntity, Guid> organizationRepository,
            IObjectMapper objectMapper,
            UserManager userManager,
            IAbpSession abpSession) : base(repository)
        {
            _locationRepository = repository;
            _objectMapper = objectMapper;
            _userManager = userManager;
            _abpSession = abpSession;
            _organizationRepository = organizationRepository;
        }


        /// <summary>
        /// Get location 
        /// </summary>
        /// <param name="input">Location Id</param>
        /// <returns>Return location</returns>
        [AbpAuthorize(PermissionNames.Pages_Locations_Get)]
        public override Task<LocationDto> GetAsync(EntityDto<Guid> input)
        {
            return base.GetAsync(input);
        }

        /// <summary>
        /// Method to get all location for current user.
        /// </summary>
        /// <param name="input">Input location parameter.</param>
        /// <returns>Return list of location.</returns>
        [AbpAuthorize(PermissionNames.Pages_Locations_List)]
        public override async Task<PagedResultDto<LocationDto>> GetAllAsync(PagedLocationResultRequestDto input)
        {
            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (roles.Contains(StaticRoleNames.Tenants.SuperAdmin))
            {
                if (input.OrganizationId.HasValue && input.OrganizationId != Guid.Empty)
                {
                    var t = Repository.GetAllIncluding(x => x.Organization).Where(x => x.OrganizationId == input.OrganizationId).PageBy(input).ToList();
                    return new PagedResultDto<LocationDto>
                    {
                        TotalCount = t.Count(),
                        Items = _objectMapper.Map<List<LocationDto>>(t)
                    };
                }
                return await base.GetAllAsync(input);
            }
            else
            {
                List<LocationEntity> t = new List<LocationEntity>();
                if (input.OrganizationId is not null)
                {
                    t = Repository.GetAllIncluding(x => x.Organization).Where(x => x.OrganizationId == input.OrganizationId && x.IsActive == true).PageBy(input).ToList();
                }
                else
                {
                    t = Repository.GetAllIncluding(x => x.Organization).Where(x => x.OrganizationId == currentUser.OrganizationId && x.IsActive == true).PageBy(input).ToList();
                }
                return new PagedResultDto<LocationDto>
                {
                    TotalCount = t.Count(),
                    Items = _objectMapper.Map<List<LocationDto>>(t)
                };
            }

        }

        /// <summary>
        /// Method to create user by location.
        /// </summary>
        /// <param name="input">Location input data.</param>
        /// <returns>Return location.</returns>
        [AbpAuthorize(PermissionNames.Pages_Locations_Add)]
        public override async Task<LocationDto> CreateAsync(CreateLocationDto input)
        {
            CheckCreatePermission();
            var location = _objectMapper.Map<LocationEntity>(input);
            await _locationRepository.InsertAsync(location);
            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(location);
        }

        /// <summary>
        /// Method to update location.
        /// </summary>
        /// <param name="location">Location data.</param>
        /// <returns>Return location data.</returns>
        [AbpAuthorize(PermissionNames.Pages_Locations_Edit)]
        public override async Task<LocationDto> UpdateAsync(LocationDto location)
        {
            CheckUpdatePermission();

            var locationData = await _locationRepository.GetAsync(location.Id);

            _objectMapper.Map(location, locationData);
            await _locationRepository.UpdateAsync(locationData);

            return MapToEntityDto(locationData);
        }

        /// <summary>
        /// Method to delete organization data.
        /// </summary>
        /// <param name="entity">Pass id as entity.</param>
        /// <returns>Return Task.</returns>
        [AbpAuthorize(PermissionNames.Pages_Locations_Delete)]
        public override async Task DeleteAsync(EntityDto<Guid> entity)
        {
            CheckDeletePermission();
            var location = await _locationRepository.GetAsync(entity.Id);
            location.IsActive = false;
            await _locationRepository.DeleteAsync(location);
        }

        /// <summary>
        /// Mehtod to get locations data.
        /// </summary>
        /// <returns>Return location list.</returns>
        public async Task<ListResultDto<LocationDto>> GetLocationsAsync()
        {
            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (roles.Contains(StaticRoleNames.Tenants.SuperAdmin))
            {
                var locations = await Repository.GetAllListAsync();
                return new ListResultDto<LocationDto>(ObjectMapper.Map<List<LocationDto>>(locations));
            }
            else if (roles.Contains(StaticRoleNames.Tenants.OrganizationITHead))
            {
                var locationsData = Repository.GetAllListAsync().Result.Where(x => x.OrganizationId == currentUser.OrganizationId || x.BranchITHeadEmail == currentUser.EmailAddress && x.IsActive == true).ToList();
                return new PagedResultDto<LocationDto>
                {
                    TotalCount = locationsData.Count(),
                    Items = _objectMapper.Map<List<LocationDto>>(locationsData)
                };
            }
            else
            {
                var locationsData = Repository.GetAllListAsync().Result.Where(x => x.OrganizationId == currentUser.OrganizationId && x.BranchITHeadEmail == currentUser.EmailAddress && x.IsActive == true).ToList();
                return new PagedResultDto<LocationDto>
                {
                    TotalCount = locationsData.Count(),
                    Items = _objectMapper.Map<List<LocationDto>>(locationsData)
                };
            }
        }

        /// <summary>
        /// Method to get locations from organization id.
        /// </summary>
        /// <param name="orgId">Organization id.</param>
        /// <returns>Returns list of location.</returns>
        public List<LocationDto> GetLocationUsingOrgId(Guid orgId)
        {
            var locationData = Repository.GetAllList().Where(x => x.OrganizationId == orgId).ToList();

            return ObjectMapper.Map<List<LocationDto>>(locationData);
        }

        /// <summary>
        /// Get location by id.
        /// </summary>
        /// <param name="locationId">Location id.</param>
        /// <returns>Return location data.</returns>
        public LocationDto GetLocationById(Guid locationId)
        {
            var location = Repository.Get(locationId);
            location.Organization = _organizationRepository.Get(location.OrganizationId);
            return ObjectMapper.Map<LocationDto>(location);
        }

        /// <summary>
        /// Method to get location for edit.
        /// </summary>
        /// <param name="input">Input as a parameter.</param>
        /// <returns>Return location data.</returns>
        public async Task<LocationDto> GetLocationForEdit(EntityDto<Guid> input)
        {
            var location = await GetAsync(input);
            return location;
        }

        /// <summary>
        /// Get base location of organization.
        /// </summary>
        /// <param name="input">Organization Id.</param>
        /// <returns>Return location.</returns>
        public async Task<LocationDto> GetBaseLocationByOrganizationAsync(EntityDto<Guid> input)
        {
            LocationEntity location = await Repository.FirstOrDefaultAsync(x => x.OrganizationId == input.Id && x.IsBaseLocation == true && x.IsActive == true);
            LocationDto locationDto = MapToEntityDto(location);
            return (locationDto);
        }

        protected override IQueryable<LocationEntity> CreateFilteredQuery(PagedLocationResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Organization)
                 .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Organization.Name.Contains(input.Keyword) || x.Address1.Contains(input.Keyword) ||
                        x.CityId.Contains(input.Keyword) || x.Landmark.Contains(input.Keyword) || x.StateId.Contains(input.Keyword) || x.CountryId.Contains(input.Keyword))
                 .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }
    }
}
