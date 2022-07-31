using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using TechEngineer.Authorization;
using TechEngineer.Authorization.Roles;
using TechEngineer.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using TechEngineer.Constants;

namespace TechEngineer.EntityFrameworkCore.Seed.Host
{
    public class HostRoleAndUserCreator
    {
        private readonly TechEngineerDbContext _context;

        public HostRoleAndUserCreator(TechEngineerDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateHostRoleAndUsers();
            CreateDefaultRoles();
        }

        private void CreateDefaultRoles()
        {
            var organizationAdminRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.OrganizationITHead);
            if (organizationAdminRoleForHost == null)
            {
                _context.Roles.Add(new Role(null, StaticRoleNames.Host.OrganizationITHead, StaticRoleNames.Host.OrganizationITHead) { IsStatic = false, IsDefault = false });
            }

            var storeITHeadRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.StoreITHead);
            if (storeITHeadRoleForHost == null)
            {
                _context.Roles.Add(new Role(null, StaticRoleNames.Host.StoreITHead, StaticRoleNames.Host.StoreITHead) { IsStatic = false, IsDefault = false });
            }

            var storeUserRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.StoreUser);
            if (storeUserRoleForHost == null)
            {
                _context.Roles.Add(new Role(null, StaticRoleNames.Host.StoreUser, StaticRoleNames.Host.StoreUser) { IsStatic = false, IsDefault = false });
            }

            var storeAdminRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.StoreAdmin);
            if (storeAdminRoleForHost == null)
            {
                _context.Roles.Add(new Role(null, StaticRoleNames.Host.StoreAdmin, StaticRoleNames.Host.StoreAdmin) { IsStatic = false, IsDefault = false });
            }

            var engineerRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Engineer);
            if (engineerRoleForHost == null)
            {
                _context.Roles.Add(new Role(null, StaticRoleNames.Host.Engineer, StaticRoleNames.Host.Engineer) { IsStatic = false, IsDefault = false });
            }

            if (organizationAdminRoleForHost == null || engineerRoleForHost == null)
            {
                _context.SaveChanges();
            }
        }

        private void CreateHostRoleAndUsers()
        {
            // Admin role for host

            var superadminRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.SuperAdmin);
            if (superadminRoleForHost == null)
            {
                superadminRoleForHost = _context.Roles.Add(new Role(null, StaticRoleNames.Host.SuperAdmin, StaticRoleNames.Host.SuperAdmin) { IsStatic = true, IsDefault = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to admin role for host

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == null && p.RoleId == superadminRoleForHost.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new TechEngineerAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = null,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = superadminRoleForHost.Id
                    })
                );
                _context.SaveChanges();
            }

            // Admin user for host
            var superadminUserForHost = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == null && u.UserName == TechEngineerUserCustomBase.SuperAdminUserName);
            if (superadminUserForHost == null)
            {
                var user = new User
                {
                    TenantId = null,
                    UserName = TechEngineerUserCustomBase.SuperAdminUserName,
                    Name = "Super",
                    Surname = "admin",
                    EmailAddress = TechEngineerUserCustomBase.SuperAdminEmailAddress,
                    IsEmailConfirmed = true,
                    IsActive = true
                };

                user.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user, "123qwe");
                user.SetNormalizedNames();

                superadminUserForHost = _context.Users.Add(user).Entity;
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(null, superadminUserForHost.Id, superadminRoleForHost.Id));
                _context.SaveChanges();

            }
        }
    }
}
