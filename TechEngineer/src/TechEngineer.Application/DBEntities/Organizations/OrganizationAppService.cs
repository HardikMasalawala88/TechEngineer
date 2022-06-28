using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
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
using TechEngineer.Constants;
using TechEngineer.DBEntities.Location;
using TechEngineer.DBEntities.Locations;
using TechEngineer.DBEntities.Locations.Dto;
using TechEngineer.DBEntities.Organization;
using TechEngineer.DBEntities.Organizations.Dto;
using TechEngineer.Users;
using TechEngineer.Users.Dto;

namespace TechEngineer.DBEntities.Organizations
{
    /// <summary>
    /// Class to define organization app service.
    /// </summary>
    //[AbpAuthorize(PermissionNames.Pages_Organizations)]
    public class OrganizationAppService : AsyncCrudAppService<OrganizationEntity, OrganizationDto, Guid, PagedOrganizationResultRequestDto, CreateOrganizationDto, OrganizationDto>, IOrganizationAppService
    {
        private readonly IRepository<OrganizationEntity, Guid> _organizationRepository;
        private readonly IRepository<LocationEntity, Guid> _locationRepository;
        private readonly IUserAppService _userAppService;
        private readonly ILocationAppService _locationAppService;
        private readonly IObjectMapper _objectMapper;
        private readonly UserManager _userManager;
        private readonly IAbpSession _abpSession;

        /// <summary>
        /// Constructor for organization service.
        /// </summary>
        /// <param name="repository">Repository.</param>
        /// <param name="locationRepository">Location Repository.</param>
        /// <param name="userAppService">User app service.</param>
        /// <param name="locationAppService">Location app service.</param>
        /// <param name="userManager">User manager.</param>
        /// <param name="abpSession">Abp session.</param>
        /// <param name="objectMapper">Object mapper.</param>
        public OrganizationAppService(IRepository<OrganizationEntity, Guid> repository,
            IRepository<LocationEntity, Guid> locationRepository,
            IUserAppService userAppService,
            ILocationAppService locationAppService,
            UserManager userManager,
            IAbpSession abpSession,
            IObjectMapper objectMapper) : base(repository)
        {
            _organizationRepository = repository;
            _locationRepository = locationRepository;
            _userAppService = userAppService;
            _locationAppService = locationAppService;
            _userManager = userManager;
            _abpSession = abpSession;
            _objectMapper = objectMapper;
        }

        /// <summary>
        /// Get organization Data.
        /// </summary>
        /// <param name="input">Entity input.</param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Organizations_Get)]
        public override async Task<OrganizationDto> GetAsync(EntityDto<Guid> input)
        {
            var organization = await _organizationRepository.FirstOrDefaultAsync(x => x.Id == input.Id && x.IsActive == true);

            OrganizationDto organizationDto = MapToEntityDto(organization);
            organizationDto.Location = _locationAppService.GetBaseLocationByOrganizationAsync(TechEngineerGlobalMethod.ToEntityDto(organization.Id)).Result;
            return (organizationDto);
        }

        /// <summary>
        /// Method to get all organization for current user.
        /// </summary>
        /// <param name="input">Input organization parameter.</param>
        /// <returns>Return list of organization.</returns>
        [AbpAuthorize(PermissionNames.Pages_Organizations_List)]
        public override async Task<PagedResultDto<OrganizationDto>> GetAllAsync(PagedOrganizationResultRequestDto input)
        {
            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (roles.Contains(StaticRoleNames.Tenants.SuperAdmin))
            {
                if (input.OrganizationId.HasValue && input.OrganizationId != Guid.Empty)
                {
                    var t = Repository.GetAll().Where(x => x.Id == input.OrganizationId).PageBy(input).ToList();
                    return new PagedResultDto<OrganizationDto>
                    {
                        TotalCount = t.Count(),
                        Items = _objectMapper.Map<List<OrganizationDto>>(t)
                    };
                }
                return await base.GetAllAsync(input);
            }
            else
            {
                var t = Repository.GetAll().Where(x => x.Id == input.OrganizationId && x.IsActive == true).PageBy(input).ToList();
                return new PagedResultDto<OrganizationDto>
                {
                    TotalCount = t.Count(),
                    Items = _objectMapper.Map<List<OrganizationDto>>(t)
                };
            }
        }

        /// <summary>
        /// Method to get list of organizations.
        /// </summary>
        /// <returns>Return list of organization.</returns>
        public async Task<ListResultDto<OrganizationDto>> GetOrganizationsAsync()
        {
            CheckGetPermission();
            List<OrganizationEntity> organizationEntities = new List<OrganizationEntity>();
            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (roles.Contains(StaticRoleNames.Tenants.SuperAdmin))
            {
                organizationEntities.Add(new OrganizationEntity { Id = Guid.Empty, Name = "All" });
                var organizationInfo = await _organizationRepository.GetAllListAsync();
                organizationInfo.ForEach(organization =>
                {
                    if (organization.IsActive)
                    {
                        organizationEntities.Add(organization);
                    }
                });
            }
            else
            {
                //User able to see their own organizatino only
                organizationEntities.Add(await _organizationRepository.GetAsync(currentUser.OrganizationId));
            }

            return new ListResultDto<OrganizationDto>(_objectMapper.Map<List<OrganizationDto>>(organizationEntities));
        }

        /// <summary>
        /// Method to create organization.
        /// </summary>
        /// <param name="input">Organization input data.</param>
        /// <returns>Return organization.</returns>
        [AbpAuthorize(PermissionNames.Pages_Organizations_Add)]
        public override async Task<OrganizationDto> CreateAsync(CreateOrganizationDto input)
        {
            CheckCreatePermission();

            var organization = _objectMapper.Map<OrganizationEntity>(input);
            organization.PrimaryEmailAddress = organization.PrimaryEmailAddress.ToLower();
            await _organizationRepository.InsertAsync(organization);
            CurrentUnitOfWork.SaveChanges();

            if (organization.Id != default)
            {
                CreateLocationDto locationDto = _objectMapper.Map<CreateLocationDto>(input.Location);
                locationDto.OrganizationId = organization.Id;
                locationDto.IsActive = true;
                locationDto.IsBaseLocation = true;
                LocationDto createdLocation = await _locationAppService.CreateAsync(locationDto); ;
                if (organization.Id != default)
                {
                    await _userAppService.CreateAsync(new CreateUserDto()
                    {
                        UserName = organization.PrimaryEmailAddress.ToLower(),
                        Name = organization.Name,
                        Surname = string.Concat("[", StaticRoleNames.Host.OrganizationITHead, "]"),
                        EmailAddress = organization.PrimaryEmailAddress.ToLower(),
                        IsActive = true,
                        RoleNames = new string[] { StaticRoleNames.Host.OrganizationITHead.ToUpper() },
                        Password = "123qwe",
                        OrganizationId = organization.Id,
                        LocationId = createdLocation.Id,
                    });
                }
                return MapToEntityDto(organization);
            }

            return MapToEntityDto(null);
        }

        /// <summary>
        /// Method to update organization data.
        /// </summary>
        /// <param name="organization">Pass organization as parameter.</param>
        /// <returns>Return organization.</returns>
        [AbpAuthorize(PermissionNames.Pages_Organizations_Edit)]
        public override async Task<OrganizationDto> UpdateAsync(OrganizationDto organization)
        {
            CheckUpdatePermission();

            var organizationData = await _organizationRepository.GetAsync(organization.Id);
            //var addressesData = await _locationRepository.GetAsync(organization.LocationId);

            //_objectMapper.Map(organization.Location, addressesData);
            //await _locationRepository.UpdateAsync(addressesData);

            _objectMapper.Map(organization, organizationData);
            await _organizationRepository.UpdateAsync(organizationData);

            return MapToEntityDto(organizationData);
        }

        /// <summary>
        /// Method to delete organization data.
        /// </summary>
        /// <param name="entity">Pass id as entity.</param>
        /// <returns>Return Task.</returns>
        [AbpAuthorize(PermissionNames.Pages_Organizations_Delete)]
        public override async Task DeleteAsync(EntityDto<Guid> entity)
        {
            CheckDeletePermission();

            var organization = await _organizationRepository.GetAsync(entity.Id);
            organization.IsActive = false;

            //var addresses = await _locationRepository.GetAsync(organization.LocationId);
            //addresses.IsActive = false;

            //await _locationRepository.DeleteAsync(addresses);
            await _organizationRepository.DeleteAsync(organization);
        }
    }
}
