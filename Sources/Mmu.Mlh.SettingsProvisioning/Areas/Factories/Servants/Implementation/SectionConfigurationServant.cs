using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants.Implementation
{
    internal class SectionConfigurationServant : ISectionConfigurationServant
    {
        public TSettings ConfigureFromSection<TSettings>(IConfigurationRoot configurationRoot, string settingsSectionKey)
            where TSettings : class, new()
        {
            var section = configurationRoot.GetSection(settingsSectionKey);

            var settingsOptions = new ServiceCollection()
                .Configure<TSettings>(section)
                .AddSingleton(configurationRoot)
                .BuildServiceProvider()
                .GetService<IOptions<TSettings>>();

            return settingsOptions.Value;
        }
    }
}