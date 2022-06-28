using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TechEngineer.Configuration;

namespace TechEngineer.Web.Host.Startup
{
    [DependsOn(
       typeof(TechEngineerWebCoreModule))]
    public class TechEngineerWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public TechEngineerWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TechEngineerWebHostModule).GetAssembly());
        }
    }
}
