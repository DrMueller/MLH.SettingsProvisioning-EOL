using Mmu.Mlh.SettingsProvisioning.Areas.Models;

namespace Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants
{
    internal interface IDirectorySearchServant
    {
        AppSettingsSearchResult SearchAppSettings(string baseDirectoryOrFilePath);
    }
}