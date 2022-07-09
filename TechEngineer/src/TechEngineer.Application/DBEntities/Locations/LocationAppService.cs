using Abp.Application.Services;
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
using System.Text;
using System.Threading.Tasks;
using TechEngineer.Authorization;
using TechEngineer.Authorization.Roles;
using TechEngineer.Authorization.Users;
using TechEngineer.DBEntities.Location;
using TechEngineer.DBEntities.Locations.Dto;
using TechEngineer.DBEntities.Organization;

namespace TechEngineer.DBEntities.Locations
{
    /// <summary>
    /// Class to define Location app service.
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_Locations)]
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

        protected override IQueryable<LocationEntity> CreateFilteredQuery(PagedLocationResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Organization)
                 .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Organization.Name.Contains(input.Keyword) || x.Address1.Contains(input.Keyword) || x.Address2.Contains(input.Keyword))
                 .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        /// <summary>
        /// Get base location of organization
        /// </summary>
        /// <param name="input">Organization Id</param>
        /// <returns>Return location</returns>
        public async Task<LocationDto> GetBaseLocationByOrganizationAsync(EntityDto<Guid> input)
        {
            LocationEntity location = await Repository.FirstOrDefaultAsync(x => x.OrganizationId == input.Id && x.IsBaseLocation == true && x.IsActive == true);
            LocationDto locationDto = MapToEntityDto(location);
            return (locationDto);
        }

        /// <summary>
        /// Get location 
        /// </summary>
        /// <param name="input">Location Id</param>
        /// <returns>Return location</returns>
        public override Task<LocationDto> GetAsync(EntityDto<Guid> input)
        {
            return base.GetAsync(input);
        }

        /// <summary>
        /// Method to get all location for current user.
        /// </summary>
        /// <param name="input">Input location parameter.</param>
        /// <returns>Return list of location.</returns>
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
                var t = Repository.GetAllIncluding(x => x.Organization).Where(x => x.OrganizationId == input.OrganizationId && x.IsActive == true).PageBy(input).ToList();
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
        //[AbpAuthorize(PermissionNames.Pages_Locations_Add)]
        public override async Task<LocationDto> CreateAsync(CreateLocationDto input)
        {
            CheckCreatePermission();

            var location = _objectMapper.Map<LocationEntity>(input);
            await _locationRepository.InsertAsync(location);
            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(location);
        }

        /// <summary>
        /// Mehtod to get locations data.
        /// </summary>
        /// <returns>Return location list.</returns>
        public async Task<ListResultDto<LocationDto>> GetLocationsAsync()
        {
            var locations = await Repository.GetAllListAsync();
            return new ListResultDto<LocationDto>(ObjectMapper.Map<List<LocationDto>>(locations));
        }
    }
}
