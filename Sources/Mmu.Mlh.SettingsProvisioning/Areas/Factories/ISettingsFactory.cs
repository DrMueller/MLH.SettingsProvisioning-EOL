namespace Mmu.Mlh.SettingsProvisioning.Areas.Factories
{
    public interface ISettingsFactory
    {
        TSettings CreateSettings<TSettings>(string settingsSectionKey, string environmentName, string baseDirectoryOrFilePath)
            where TSettings : class, new();
    }
}