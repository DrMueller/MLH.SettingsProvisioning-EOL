using System;
using System.Globalization;
using System.IO.Abstractions;
using System.Linq;
using Mmu.Mlh.SettingsProvisioning.Areas.Models;

namespace Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants.Implementation
{
    internal class DirectorySearchServant : IDirectorySearchServant
    {
        private readonly IFileSystem _fileSystem;

        public DirectorySearchServant(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public AppSettingsSearchResult SearchAppSettings(string baseDirectoryOrFilePath)
        {
            var baseDirectory = AlignBaseDirectory(baseDirectoryOrFilePath);
            if (string.IsNullOrEmpty(baseDirectory))
            {
                return new AppSettingsSearchResult(false, string.Empty);
            }

            var currentFolder = _fileSystem.DirectoryInfo.FromDirectoryName(baseDirectory);
            while (true)
            {
                // Reached the top level folder
                if (currentFolder == null)
                {
                    return new AppSettingsSearchResult(false, string.Empty);
                }

                var hasAppSettings = currentFolder
                    .EnumerateFiles()
                    .Any(
                        f => f.Name.StartsWith("AppSettings", StringComparison.OrdinalIgnoreCase)
                            && f.Extension.ToUpper(CultureInfo.InvariantCulture) == ".JSON");

                if (hasAppSettings)
                {
                    return new AppSettingsSearchResult(true, currentFolder.FullName);
                }

                currentFolder = currentFolder.Parent;
            }
        }

        private string AlignBaseDirectory(string baseDirectoryOrFilePath)
        {
            if (!_fileSystem.File.Exists(baseDirectoryOrFilePath))
            {
                var seperator = _fileSystem.Path.DirectorySeparatorChar.ToString();
                if (!baseDirectoryOrFilePath.EndsWith(seperator, StringComparison.Ordinal))
                {
                    baseDirectoryOrFilePath += seperator;
                }
            }

            var uri = new UriBuilder(baseDirectoryOrFilePath);
            var unescapedUri = Uri.UnescapeDataString(uri.Path);
            var directoryName = _fileSystem.Path.GetDirectoryName(unescapedUri);

            if (_fileSystem.Directory.Exists(directoryName))
            {
                return directoryName;
            }

            return string.Empty;
        }
    }
}