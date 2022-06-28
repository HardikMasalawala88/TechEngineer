using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TechEngineer.EntityFrameworkCore;
using TechEngineer.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace TechEngineer.Web.Tests
{
    [DependsOn(
        typeof(TechEngineerWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class TechEngineerWebTestModule : AbpModule
    {
        public TechEngineerWebTestModule(TechEngineerEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TechEngineerWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(TechEngineerWebMvcModule).Assembly);
        }
    }
}