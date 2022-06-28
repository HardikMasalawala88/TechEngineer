using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TechEngineer.Configuration;

namespace TechEngineer.Web.Startup
{
    [DependsOn(typeof(TechEngineerWebCoreModule))]
    public class TechEngineerWebMvcModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public TechEngineerWebMvcModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = _env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<TechEngineerNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TechEngineerWebMvcModule).GetAssembly());
        }
    }
}
