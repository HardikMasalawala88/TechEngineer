using System;
using System.Collections.Generic;
using Abp.Authorization.Users;
using Abp.Extensions;
using TechEngineer.Constants;

namespace TechEngineer.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        public Guid OrganizationId { get; set; }
        public Guid LocationId { get; set; }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                //UserName = AdminUserName,
                //Name = AdminUserName,
                //Surname = AdminUserName,
                UserName = TechEngineerUserCustomBase.SuperAdminUserName,
                Name = TechEngineerUserCustomBase.SuperAdminUserName,
                Surname = TechEngineerUserCustomBase.SuperAdminUserName,
                EmailAddress = emailAddress,
                OrganizationId = Guid.Empty,
                LocationId = Guid.Empty,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
