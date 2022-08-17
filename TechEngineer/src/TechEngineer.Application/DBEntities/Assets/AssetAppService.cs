using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
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
using TechEngineer.DBEntities.Assets.Dto;
using TechEngineer.DBEntities.Location;
using TechEngineer.DBEntities.Locations;
using TechEngineer.DBEntities.Organizations;
using TechEngineer.Users;

namespace TechEngineer.DBEntities.Assets
{
    /// <summary>
    /// Class to define asset app service.
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_Assets)]
    public class AssetAppService : AsyncCrudAppService<AssetEntity, AssetDto, Guid, PagedAssetResultRequestDto, CreateAssetDto, AssetDto>, IAssetAppService
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IRepository<AssetEntity, Guid> _assetsRepository;
        private readonly UserManager _userManager;
        private readonly IAbpSession _abpSession;

        /// <summary>
        /// Constructor for asset service.
        /// </summary>
        /// <param name="repository">Repository for asset entity.</param>
        /// <param name="objectMapper">Object mapper.</param>
        public AssetAppService(IRepository<AssetEntity, Guid> repository,
            IAbpSession abpSession,
            UserManager userManager,
            IObjectMapper objectMapper) : base(repository)
        {
            _objectMapper = objectMapper;
            _assetsRepository = repository;
            _userManager = userManager;
            _abpSession = abpSession;
        }

        /// <summary>
        /// Method to create asset.
        /// </summary>
        /// <param name="input">asset input data.</param>
        /// <returns>Return asset.</returns>
        [AbpAuthorize(PermissionNames.Pages_Assets_Add)]
        public override async Task<AssetDto> CreateAsync(CreateAssetDto input)
        {
            CheckCreatePermission();
            var asset = _objectMapper.Map<AssetEntity>(input);
            await _assetsRepository.InsertAsync(asset);
            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(asset);
        }

        [AbpAuthorize(PermissionNames.Pages_Assets_Edit)]
        public override async Task<AssetDto> UpdateAsync(AssetDto asset)
        {
            CheckUpdatePermission();

            var assetData = await _assetsRepository.GetAsync(asset.Id);

            _objectMapper.Map(asset, assetData);
            await _assetsRepository.UpdateAsync(assetData);

            return MapToEntityDto(assetData);
        }

        [AbpAuthorize(PermissionNames.Pages_Assets_List)]
        public override async Task<PagedResultDto<AssetDto>> GetAllAsync(PagedAssetResultRequestDto input)
        {
            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (roles.Contains(StaticRoleNames.Tenants.SuperAdmin))
            {
                if (input.OrganizationId.HasValue && input.OrganizationId != Guid.Empty)
                {
                    var t = (input.LocationId.HasValue && input.LocationId != Guid.Empty) ?
                    Repository.GetAll().Where(x => x.LocationId == input.LocationId).PageBy(input).ToList() :
                    Repository.GetAll().Where(x => x.OrganizationId == input.OrganizationId).PageBy(input).ToList();

                    return new PagedResultDto<AssetDto>
                    {
                        TotalCount = t.Count(),
                        Items = _objectMapper.Map<List<AssetDto>>(t)
                    };
                }
                return await base.GetAllAsync(input);
            }
            else if (roles.Contains(StaticRoleNames.Tenants.OrganizationITHead))
            {
                var t = (input.LocationId.HasValue && input.LocationId != Guid.Empty) ?
                    Repository.GetAll().Where(x => x.LocationId == input.LocationId && x.IsActive == true).PageBy(input).ToList() :
                Repository.GetAll().Where(x => x.OrganizationId == currentUser.OrganizationId || x.LocationId == currentUser.LocationId && x.IsActive == true).PageBy(input).ToList();
                return new PagedResultDto<AssetDto>
                {
                    TotalCount = t.Count(),
                    Items = _objectMapper.Map<List<AssetDto>>(t)
                };
            }
            else
            {
                var t = (input.LocationId.HasValue && input.LocationId != Guid.Empty) ?
                    Repository.GetAll().Where(x => x.LocationId == input.LocationId && x.IsActive == true).PageBy(input).ToList() :
                Repository.GetAll().Where(x => x.OrganizationId == currentUser.OrganizationId && x.LocationId == currentUser.LocationId && x.IsActive == true).PageBy(input).ToList();
                return new PagedResultDto<AssetDto>
                {
                    TotalCount = t.Count(),
                    Items = _objectMapper.Map<List<AssetDto>>(t)
                };
            }
        }

        /// <summary>
        /// Method to get list of assets.
        /// </summary>
        /// <returns>Return list of assets.</returns>
        public async Task<ListResultDto<AssetDto>> GetAssetsAsync()
        {
            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (roles.Contains(StaticRoleNames.Tenants.SuperAdmin))
            {
                var assets = await Repository.GetAllListAsync();
                return new ListResultDto<AssetDto>(ObjectMapper.Map<List<AssetDto>>(assets));
            }
            else if (roles.Contains(StaticRoleNames.Tenants.OrganizationITHead))
            {
                var assetsData = Repository.GetAllListAsync().Result.Where(x => x.OrganizationId == currentUser.OrganizationId || x.LocationId == currentUser.LocationId && x.IsActive == true).ToList();
                return new PagedResultDto<AssetDto>
                {
                    TotalCount = assetsData.Count(),
                    Items = _objectMapper.Map<List<AssetDto>>(assetsData)
                };
            }
            else
            {
                var assetsData = Repository.GetAllListAsync().Result.Where(x => x.OrganizationId == currentUser.OrganizationId && x.LocationId == currentUser.LocationId && x.IsActive == true).ToList();
                return new PagedResultDto<AssetDto>
                {
                    TotalCount = assetsData.Count(),
                    Items = _objectMapper.Map<List<AssetDto>>(assetsData)
                };
            }
        }

        /// <summary>
        /// Get asset for edit.
        /// </summary>
        /// <param name="input">Input parameter.</param>
        /// <returns>Return assets data.</returns>
        public async Task<AssetDto> GetAssetForEdit(EntityDto<Guid> input)
        {
            var asset = await GetAsync(input);
            return asset;
        }

        protected override IQueryable<AssetEntity> CreateFilteredQuery(PagedAssetResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Location)
                 .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Organization.Name.Contains(input.Keyword) || x.Name.Contains(input.Keyword) ||
                        x.Category.Contains(input.Keyword) || x.SerialNumber.Contains(input.Keyword) || x.ModelNumber.Contains(input.Keyword))
                 .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        /// <summary>
        /// Method to get assets from location id.
        /// </summary>
        /// <param name="locationId">Location id.</param>
        /// <returns>Returns list of assets.</returns>
        public List<AssetDto> GetAssetsUsingLocationId(Guid locationId)
        {
            var assets = Repository.GetAllList().Where(x => x.LocationId == locationId).ToList();

            return ObjectMapper.Map<List<AssetDto>>(assets);
        }
    }
}
