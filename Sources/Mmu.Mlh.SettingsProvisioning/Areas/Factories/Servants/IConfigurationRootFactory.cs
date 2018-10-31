using Microsoft.Extensions.Configuration;

namespace Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants
{
    public interface IConfigurationRootFactory
    {
        IConfigurationRoot Create(string appSettingsPath, string environmentName);
    }
}