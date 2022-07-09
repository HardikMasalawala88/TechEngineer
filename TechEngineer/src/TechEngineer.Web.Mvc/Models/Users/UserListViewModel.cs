using System.Collections.Generic;
using TechEngineer.Roles.Dto;
using TechEngineer.Users.Dto;

namespace TechEngineer.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
        public IReadOnlyList<UserDto> Users { get; set; }
    }
}
