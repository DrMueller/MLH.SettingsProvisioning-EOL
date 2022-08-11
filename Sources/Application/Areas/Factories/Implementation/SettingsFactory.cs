using System.IO.Abstractions;
using Microsoft.Extensions.Configuration;
using Mmu.Mlh.ApplicationExtensions.Areas.Dropbox.Services;
using Mmu.Mlh.LanguageExtensions.Areas.Types.Maybes;
using Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants;
using Mmu.Mlh.SettingsProvisioning.Areas.Models;
using Mmu.Mlh.SettingsProvisioning.Infrastructure.Exceptions;

namespace Mmu.Mlh.SettingsProvisioning.Areas.Factories.Implementation
{
    internal class SettingsFactory : ISettingsFactory
    {
        private readonly IDirectorySearchServant _directorySearchServant;
        private readonly IDropboxLocator _dropboxLocator;
        private readonly IFileSystem _fileSystem;

        public SettingsFactory(
            IDirectorySearchServant directorySearchServant,
            IDropboxLocator dropboxLocator,
            IFileSystem fileSystem)
        {
            _directorySearchServant = directorySearchServant;
            _dropboxLocator = dropboxLocator;
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

            var configBuilder = new ConfigurationBuilder();

            var configRoot = configBuilder
                .SetBasePath(appSettingsSearchResult.AppSettingsPath)
                .AddJsonFile("appsettings.json", false, false)
                .AddJsonFile($"appsettings.{config.EnvironmentName}.json", true)
                .Build();

            var settings = new TSettings();
            configRoot
                .GetSection(config.SettingsSectionKey)
                .Bind(settings);

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

        private AppSettingsSearchResult SearchDropboxSettings(SettingsConfiguration config)
        {
            if (string.IsNullOrEmpty(config.DropboxRelativePath))
            {
                return new AppSettingsSearchResult(false, string.Empty);
            }

            var dropboxPath = _dropboxLocator.LocateDropboxPath().Reduce(() => string.Empty);

            if (string.IsNullOrEmpty(dropboxPath))
            {
                return new AppSettingsSearchResult(false, string.Empty);
            }

            var relativeDropboxPath = _fileSystem.Path.Combine(dropboxPath, config.DropboxRelativePath);

            return _directorySearchServant.SearchAppSettings(relativeDropboxPath);
        }
    }
}