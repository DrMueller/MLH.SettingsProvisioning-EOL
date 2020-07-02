namespace Mmu.Mlh.SettingsProvisioning.Areas.Models
{
    internal class AppSettingsSearchResult
    {
        public AppSettingsSearchResult(bool foundAppSettings, string appSettingsPath)
        {
            FoundAppSettings = foundAppSettings;
            AppSettingsPath = appSettingsPath;
        }

        public string AppSettingsPath { get; }
        public bool FoundAppSettings { get; }
    }
}