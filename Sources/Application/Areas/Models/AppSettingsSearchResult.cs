namespace Mmu.Mlh.SettingsProvisioning.Areas.Models
{
    internal class AppSettingsSearchResult
    {
        public string AppSettingsPath { get; }
        public bool FoundAppSettings { get; }

        public AppSettingsSearchResult(bool foundAppSettings, string appSettingsPath)
        {
            FoundAppSettings = foundAppSettings;
            AppSettingsPath = appSettingsPath;
        }
    }
}