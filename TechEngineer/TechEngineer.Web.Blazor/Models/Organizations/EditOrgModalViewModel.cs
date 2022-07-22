using System.Collections.Generic;
using TechEngineer.DBEntities.Locations.Dto;
using TechEngineer.DBEntities.Organizations.Dto;

namespace TechEngineer.Web.Models.Organizations
{
    public class EditOrgModalViewModel
    {
        public OrganizationDto Organization { get; set; }

        public LocationDto Locations  { get; set; }

        public bool OrganizationIsInLocation(LocationDto location)
        {
            return Organization.Location != null && Organization.Name == location.Organization.Name;
        }
    }
}
