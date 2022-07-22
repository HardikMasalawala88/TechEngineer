using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
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
    }
}
