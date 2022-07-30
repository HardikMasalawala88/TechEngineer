using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace TechEngineer.Authorization
{
    public class TechEngineerAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Add, L("Users" + PA(1)));
            context.CreatePermission(PermissionNames.Pages_Users_Edit, L("Users" + PA(2)));
            context.CreatePermission(PermissionNames.Pages_Users_Delete, L("Users" + PA(3)));
            context.CreatePermission(PermissionNames.Pages_Users_List, L("Users" + PA(4)));
            context.CreatePermission(PermissionNames.Pages_Users_Get, L("Users" + PA(5)));

            context.CreatePermission(PermissionNames.Pages_Organizations, L("Organizations"));
            context.CreatePermission(PermissionNames.Pages_Organizations_Add, L("Organizations" + PA(1)));
            context.CreatePermission(PermissionNames.Pages_Organizations_Edit, L("Organizations" + PA(2)));
            context.CreatePermission(PermissionNames.Pages_Organizations_Delete, L("Organizations" + PA(3)));
            context.CreatePermission(PermissionNames.Pages_Organizations_List, L("Organizations" + PA(4)));
            context.CreatePermission(PermissionNames.Pages_Organizations_Get, L("Organizations" + PA(5)));

            context.CreatePermission(PermissionNames.Pages_Locations, L("Locations"));
            context.CreatePermission(PermissionNames.Pages_Locations_Add, L("Locations" + PA(1)));
            context.CreatePermission(PermissionNames.Pages_Locations_Edit, L("Locations" + PA(2)));
            context.CreatePermission(PermissionNames.Pages_Locations_Delete, L("Locations" + PA(3)));
            context.CreatePermission(PermissionNames.Pages_Locations_List, L("Locations" + PA(4)));
            context.CreatePermission(PermissionNames.Pages_Locations_Get, L("Locations" + PA(5)));
            context.CreatePermission(PermissionNames.Pages_Locations_Add_StoreItHead, L("AddStoreItHeadFromLocation"));
            context.CreatePermission(PermissionNames.Pages_Locations_Add_StoreUser, L("AddStoreUserFromLocation"));

            context.CreatePermission(PermissionNames.Pages_Assets, L("Assets"));
            context.CreatePermission(PermissionNames.Pages_Assets_Add, L("Assets" + PA(1)));
            context.CreatePermission(PermissionNames.Pages_Assets_Edit, L("Assets" + PA(2)));
            context.CreatePermission(PermissionNames.Pages_Assets_Delete, L("Assets" + PA(3)));
            context.CreatePermission(PermissionNames.Pages_Assets_List, L("Assets" + PA(4)));
            context.CreatePermission(PermissionNames.Pages_Assets_Get, L("Assets" + PA(5)));

            context.CreatePermission(PermissionNames.Pages_Appointments, L("Appointments"));
            context.CreatePermission(PermissionNames.Pages_Appointments_Add, L("Appointments" + PA(1)));
            context.CreatePermission(PermissionNames.Pages_Appointments_Edit, L("Appointments" + PA(2)));
            context.CreatePermission(PermissionNames.Pages_Appointments_Delete, L("Appointments" + PA(3)));
            context.CreatePermission(PermissionNames.Pages_Appointments_List, L("Appointments" + PA(4)));
            context.CreatePermission(PermissionNames.Pages_Appointments_Get, L("Appointments" + PA(5)));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TechEngineerConsts.LocalizationSourceName);
        }

        /// <summary>
        /// Predefined actions(PA).
        /// </summary>
        /// <param name="action">Action name.</param>
        /// <returns>Return action value.</returns>
        private string PA(int action)
        {

            switch (action)
            {
                case 1: return "_ADD";
                case 2: return "_EDIT";
                case 3: return "_DELETE";
                case 4: return "_LIST";
                case 5: return "_GET";
                default:
                    return "_LIST";
            }
        }
    }
}
