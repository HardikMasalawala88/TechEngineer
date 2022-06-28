using TechEngineer.Debugging;

namespace TechEngineer
{
    public class TechEngineerConsts
    {
        public const string LocalizationSourceName = "TechEngineer";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "f08f8579c79f475fa78c1cf81da5ff9b";
    }
}
