using System.Collections.Generic;
using TechEngineer.Roles.Dto;

namespace TechEngineer.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
