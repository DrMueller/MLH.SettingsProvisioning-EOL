using Microsoft.Extensions.Configuration;

namespace Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants
{
    public interface ISectionConfigurationServant
    {
        TSettings ConfigureFromSection<TSettings>(IConfigurationRoot configurationRoot, string settingsSectionKey)
            where TSettings : class, new();
    }
}