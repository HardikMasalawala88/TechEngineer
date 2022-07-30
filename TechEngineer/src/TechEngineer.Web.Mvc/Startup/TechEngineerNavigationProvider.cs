using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using TechEngineer.Authorization;

namespace TechEngineer.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class TechEngineerNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                //.AddItem(
                //    new MenuItemDefinition(
                //        PageNames.About,
                //        L("About"),
                //        url: "About",
                //        icon: "fas fa-info-circle"
                //    )
                //)
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Home,
                        L("Dashboard"),
                        url: "",
                        icon: "fas fa-home",
                        requiresAuthentication: true
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Tenants,
                        L("Tenants"),
                        url: "Tenants",
                        icon: "fas fa-building",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Users,
                        L("Users"),
                        url: "Users",
                        icon: "fas fa-users",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Roles,
                        L("Roles"),
                        url: "Roles",
                        icon: "fas fa-theater-masks",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Organizations,
                        L("Organizations"),
                        url: "Organizations",
                        icon: "fas fa-building",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Organizations)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Locations,
                        L("Locations"),
                        url: "Locations",
                        icon: "fas fa-map-marker",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Locations)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Assets,
                        L("Assets"),
                        url: "Assets",
                        icon: "fas fa-building",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Assets)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Appointments,
                        L("Appointments"),
                        url: "Appointments",
                        icon: "fas fa-building",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Appointments)
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TechEngineerConsts.LocalizationSourceName);
        }
    }
}