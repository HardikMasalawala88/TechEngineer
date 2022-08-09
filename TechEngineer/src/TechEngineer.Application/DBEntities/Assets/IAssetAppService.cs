using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechEngineer.DBEntities.Assets.Dto;

namespace TechEngineer.DBEntities.Assets
{
    /// <summary>
    /// Interface for asset app service.
    /// </summary>
    public interface IAssetAppService : IAsyncCrudAppService<AssetDto, Guid, PagedAssetResultRequestDto, CreateAssetDto, AssetDto>
    {
        /// <summary>
        /// Method to get assets.
        /// </summary>
        /// <returns>Return list of asset.</returns>
        Task<ListResultDto<AssetDto>> GetAssetsAsync();

        /// <summary>
        /// Get asset for edit.
        /// </summary>
        /// <param name="input">Input parameter.</param>
        /// <returns>Return assets data.</returns>
        Task<AssetDto> GetAssetForEdit(EntityDto<Guid> input);

        /// <summary>
        /// Method to get assets from location id.
        /// </summary>
        /// <param name="locationId">Location id.</param>
        /// <returns>Returns list of assets.</returns>
        List<AssetDto> GetAssetsUsingLocationId(Guid locationId);
    }
}
