using System.Collections.Generic;
using TechEngineer.Roles.Dto;

namespace TechEngineer.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
