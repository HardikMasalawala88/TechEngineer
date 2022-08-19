using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace TechEngineer.Localization
{
    public static class TechEngineerLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(TechEngineerConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(TechEngineerLocalizationConfigurer).GetAssembly(),
                        "TechEngineer.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
