using Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants;
using Mmu.Mlh.SettingsProvisioning.Infrastructure.Exceptions;

namespace Mmu.Mlh.SettingsProvisioning.Areas.Factories.Implementation
{
    internal class SettingsFactory : ISettingsFactory
    {
        private readonly IConfigurationRootFactory _configurationRootFactory;
        private readonly IDirectorySearchServant _directorySearchServant;
        private readonly ISectionConfigurationServant _sectionConfigurationServant;

        public SettingsFactory(
            IDirectorySearchServant directorySearchServant,
            IConfigurationRootFactory configurationRootFactory,
            ISectionConfigurationServant sectionConfigurationServant)
        {
            _directorySearchServant = directorySearchServant;
            _configurationRootFactory = configurationRootFactory;
            _sectionConfigurationServant = sectionConfigurationServant;
        }

        public TSettings CreateSettings<TSettings>(
            string settingsSectionKey,
            string environmentName,
            string baseDirectoryOrFilePath)
            where TSettings : class, new()
        {
            var appSettingsSearchResult = _directorySearchServant.SearchAppSettings(baseDirectoryOrFilePath);
            if (!appSettingsSearchResult.FoundAppSettings)
            {
                throw new AppSettingsNotFoundException(baseDirectoryOrFilePath);
            }

            var configurationRoot = _configurationRootFactory.Create(appSettingsSearchResult.AppSettingsPath, environmentName);
            var settings = _sectionConfigurationServant.ConfigureFromSection<TSettings>(configurationRoot, settingsSectionKey);
            return settings;
        }
    }
}