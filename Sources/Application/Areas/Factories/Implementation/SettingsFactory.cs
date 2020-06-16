using System;
using System.IO.Abstractions;
using Mmu.Mlh.LanguageExtensions.Areas.Types.FunctionsResults;
using Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants;
using Mmu.Mlh.SettingsProvisioning.Areas.Models;
using Mmu.Mlh.SettingsProvisioning.Infrastructure.Exceptions;

namespace Mmu.Mlh.SettingsProvisioning.Areas.Factories.Implementation
{
    internal class SettingsFactory : ISettingsFactory
    {
        private readonly IConfigurationRootFactory _configurationRootFactory;
        private readonly IDirectorySearchServant _directorySearchServant;
        private readonly IFileSystem _fileSystem;
        private readonly ISectionConfigurationServant _sectionConfigurationServant;

        public SettingsFactory(
            IDirectorySearchServant directorySearchServant,
            IConfigurationRootFactory configurationRootFactory,
            ISectionConfigurationServant sectionConfigurationServant,
            IFileSystem fileSystem)
        {
            _directorySearchServant = directorySearchServant;
            _configurationRootFactory = configurationRootFactory;
            _sectionConfigurationServant = sectionConfigurationServant;
            _fileSystem = fileSystem;
        }

        public TSettings CreateSettings<TSettings>(SettingsConfiguration config)
            where TSettings : class, new()
        {
            var appSettingsSearchResult = SearchAppSettings(config);
            if (!appSettingsSearchResult.FoundAppSettings)
            {
                throw new AppSettingsNotFoundException(config.BaseDirectoryOrFilePath);
            }

            var configurationRoot = _configurationRootFactory.Create(appSettingsSearchResult.AppSettingsPath, config.EnvironmentName);
            var settings = _sectionConfigurationServant.ConfigureFromSection<TSettings>(configurationRoot, config.SettingsSectionKey);
            return settings;
        }

        private AppSettingsSearchResult SearchAppSettings(SettingsConfiguration config)
        {
            // If we find a Dropbox file, we're locally and use this one
            var dropboxSearchResult = SearchDropboxSettings(config);
            if (dropboxSearchResult.FoundAppSettings)
            {
                return dropboxSearchResult;
            }

            // Otherwise, the default path
            var appSettingsSearchResult = _directorySearchServant.SearchAppSettings(config.BaseDirectoryOrFilePath);
            return appSettingsSearchResult;
        }

        private FunctionResult<string> SearchDropboxBasePath()
        {
            const string DropboxInfoPath = @"Dropbox\info.json";
            var jsonPath = _fileSystem.Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), DropboxInfoPath);

            if (!_fileSystem.File.Exists(jsonPath))
            {
                jsonPath = _fileSystem.Path.Combine(Environment.GetEnvironmentVariable("AppData"), DropboxInfoPath);
            }

            if (!_fileSystem.File.Exists(jsonPath))
            {
                return FunctionResult.CreateFailure<string>();
            }

            var dropboxPath = _fileSystem.File.ReadAllText(jsonPath).Split('\"')[5].Replace(@"\\", @"\");
            return FunctionResult.CreateSuccess(dropboxPath);
        }

        private AppSettingsSearchResult SearchDropboxSettings(SettingsConfiguration config)
        {
            if (string.IsNullOrEmpty(config.DropboxRelativePath))
            {
                return new AppSettingsSearchResult(false, string.Empty);
            }

            var dropboxBasePathSearchResult = SearchDropboxBasePath();
            if (!dropboxBasePathSearchResult.IsSuccess)
            {
                return new AppSettingsSearchResult(false, string.Empty);
            }

            var relativeDropboxPath = _fileSystem.Path.Combine(
                dropboxBasePathSearchResult.Value,
                config.DropboxRelativePath);

            return _directorySearchServant.SearchAppSettings(relativeDropboxPath);
        }
    }
}