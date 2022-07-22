﻿using Abp.Application.Services;
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
        private readonly IOrganizationAppService _organizationAppService;
        private readonly ILocationAppService _locationAppService;
        private readonly IUserAppService _userAppService;
        private readonly UserManager _userManager;
        private readonly IAbpSession _abpSession;

        /// <summary>
        /// Constructor for asset service.
        /// </summary>
        /// <param name="repository">Repository for asset entity.</param>
        /// <param name="objectMapper">Object mapper.</param>
        public AssetAppService(IRepository<AssetEntity, Guid> repository,
            IOrganizationAppService organizationAppService,
            ILocationAppService locationAppService,
            IUserAppService userAppService,
            IAbpSession abpSession,
            UserManager userManager,
            IObjectMapper objectMapper) : base(repository)
        {
            _objectMapper = objectMapper;
            _organizationAppService = organizationAppService;
            _locationAppService = locationAppService;
            _assetsRepository = repository;
            _userManager = userManager;
            _userAppService = userAppService;
            _abpSession = abpSession;
        }

        /// <summary>
        /// Method to create asset.
        /// </summary>
        /// <param name="input">asset input data.</param>
        /// <returns>Return asset.</returns>
        //[AbpAuthorize(PermissionNames.Pages_Assets_Add)]
        public override async Task<AssetDto> CreateAsync(CreateAssetDto input)
        {
            CheckCreatePermission();

            var asset = _objectMapper.Map<AssetEntity>(input);
            await _assetsRepository.InsertAsync(asset);
            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(asset);
        }

        public override async Task<AssetDto> UpdateAsync(AssetDto asset)
        {
            var assetData = await _assetsRepository.GetAsync(asset.Id);

            _objectMapper.Map(asset, assetData);
            await _assetsRepository.UpdateAsync(assetData);

            return MapToEntityDto(assetData);
        }

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
            else
            {
                var t = (input.LocationId.HasValue && input.LocationId != Guid.Empty) ?
                    Repository.GetAll().Where(x => x.LocationId == input.LocationId && x.IsActive == true).PageBy(input).ToList() :
                Repository.GetAll().Where(x => x.OrganizationId == input.OrganizationId && x.IsActive == true).PageBy(input).ToList();
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
            CheckGetPermission();
            List<AssetEntity> assetEntities = new List<AssetEntity>();
            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (roles.Contains(StaticRoleNames.Tenants.SuperAdmin))
            {
                //var locationData = await _locationAppService.GetLocationsAsync();
                assetEntities.Add(new AssetEntity { Id = Guid.Empty, Name = "All" });
                var assetInfo = await _assetsRepository.GetAllListAsync();
                assetInfo.ForEach(asset =>
                {
                    if (asset.IsActive)
                    {
                        //asset.Location = (LocationEntity)locationData.Items;
                        assetEntities.Add(asset);
                    }
                });
            }
            else
            {
                //User able to see their own organization only
                assetEntities.Add(await _assetsRepository.GetAsync(currentUser.OrganizationId));
            }

            return new ListResultDto<AssetDto>(_objectMapper.Map<List<AssetDto>>(assetEntities));
        }
    }
}
