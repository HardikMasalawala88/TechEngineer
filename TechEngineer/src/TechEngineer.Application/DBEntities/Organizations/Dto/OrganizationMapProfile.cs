using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechEngineer.DBEntities.Organization;

namespace TechEngineer.DBEntities.Organizations.Dto
{
    /// <summary>
    /// Class to define organization map profile.
    /// </summary>
    public class OrganizationMapProfile : Profile
    {
        /// <summary>
        /// Constructor for organization map profile.
        /// </summary>
        public OrganizationMapProfile()
        {
            CreateMap<OrganizationDto, OrganizationEntity>();
            CreateMap<OrganizationDto, OrganizationEntity>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateOrganizationDto, OrganizationEntity>();
            CreateMap<CreateOrganizationDto, OrganizationEntity>();
        }
    }
}
