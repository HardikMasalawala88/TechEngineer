using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechEngineer.DBEntities.Location;

namespace TechEngineer.DBEntities.Locations.Dto
{
    /// <summary>
    /// Class to define location map profile.
    /// </summary>
    public class LocationMapProfile : Profile
    {
        /// <summary>
        /// Constructor for location map profile.
        /// </summary>
        public LocationMapProfile()
        {
            CreateMap<LocationDto, LocationEntity>();
            CreateMap<LocationDto, LocationEntity>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateLocationDto, LocationEntity>();
            CreateMap<CreateLocationDto, LocationEntity>();
        }
    }
}
