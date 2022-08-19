using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechEngineer.DBEntities.Assets.Dto
{
    /// <summary>
    /// Class to map asset.
    /// </summary>
    public class AssetMapProfile : Profile
    {
        /// <summary>
        /// Constructor for asset map profile.
        /// </summary>
        public AssetMapProfile()
        {
            CreateMap<AssetDto, AssetEntity>();
            CreateMap<AssetDto, AssetEntity>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateAssetDto, AssetEntity>();
            CreateMap<CreateAssetDto, AssetEntity>();
        }
    }
}
