using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TechEngineer.Authorization;

namespace TechEngineer
{
    [DependsOn(
        typeof(TechEngineerCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class TechEngineerApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<TechEngineerAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(TechEngineerApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
