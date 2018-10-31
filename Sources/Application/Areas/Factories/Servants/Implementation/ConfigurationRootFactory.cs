using Microsoft.Extensions.Configuration;

namespace Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants.Implementation
{
    internal class ConfigurationRootFactory : IConfigurationRootFactory
    {
        public IConfigurationRoot Create(string appSettingsPath, string environmentName)
        {
            return new ConfigurationBuilder()
                .SetBasePath(appSettingsPath)
                .AddJsonFile("appsettings.json", false, false)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .Build();
        }
    }
}