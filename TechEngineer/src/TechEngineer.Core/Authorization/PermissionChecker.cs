using Abp.Authorization;
using TechEngineer.Authorization.Roles;
using TechEngineer.Authorization.Users;

namespace TechEngineer.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
