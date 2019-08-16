using Mmu.Mlh.LanguageExtensions.Areas.Invariance;

namespace Mmu.Mlh.SettingsProvisioning.Areas.Models
{
    public class SettingsConfiguration
    {
        public string BaseDirectoryOrFilePath { get; }
        public string DropboxRelativePath { get; }
        public string EnvironmentName { get; }
        public string SettingsSectionKey { get; }

        public SettingsConfiguration(
            string settingsSectionKey,
            string environmentName,
            string baseDirectoryOrFilePath,
            string dropboxRelativePath = null)
        {
            Guard.StringNotNullOrEmpty(() => settingsSectionKey);

            SettingsSectionKey = settingsSectionKey;
            EnvironmentName = environmentName;
            BaseDirectoryOrFilePath = baseDirectoryOrFilePath;
            DropboxRelativePath = dropboxRelativePath;
        }
    }
}