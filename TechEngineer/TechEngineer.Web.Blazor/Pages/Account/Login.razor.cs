using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Notifications;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using TechEngineer.Authorization;
using TechEngineer.Authorization.Users;
using TechEngineer.Identity;
using TechEngineer.MultiTenancy;
using TechEngineer.Sessions;
using TechEngineer.Web.Models.Account;

namespace TechEngineer.Web.Blazor.Pages.Account
{
    public partial class Login
    {
        private readonly IAbpSession _abpSession;
        public LogInManager LogInManager { get; set; }
        
        public ITenantCache TenantCache { get; set; }
        
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }

        public AbpLoginResultTypeHelper AbpLoginResultTypeHelper { get; set; }

        public SignInManager SignInManager { get; set; }

        LoginViewModel LoginViewModel = new LoginViewModel();

        public async Task OnValidSubmitAsync() 
        {
            var loginResult = await GetLoginResultAsync(LoginViewModel.UsernameOrEmailAddress, LoginViewModel.Password, GetTenancyNameOrNull());

            await SignInManager.SignInAsync(loginResult.Identity, LoginViewModel.RememberMe);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            NavManager.NavigateTo("/home");
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await LogInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw AbpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private string GetTenancyNameOrNull()
        {
            return null;
            if (!_abpSession.TenantId.HasValue)
            {
                return null;
            }

            return TenantCache.GetOrNull(_abpSession.TenantId.Value)?.TenancyName;
        }
    }
}
