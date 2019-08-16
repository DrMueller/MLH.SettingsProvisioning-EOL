using Mmu.Mlh.SettingsProvisioning.Areas.Models;

namespace Mmu.Mlh.SettingsProvisioning.Areas.Factories
{
    public interface ISettingsFactory
    {
        TSettings CreateSettings<TSettings>(SettingsConfiguration config)
            where TSettings : class, new();
    }
}