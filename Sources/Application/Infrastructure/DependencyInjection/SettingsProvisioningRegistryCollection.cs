using System.IO.Abstractions;
using Lamar;
using Mmu.Mlh.SettingsProvisioning.Areas.Factories;
using Mmu.Mlh.SettingsProvisioning.Areas.Factories.Implementation;
using Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants;
using Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants.Implementation;

namespace Mmu.Mlh.SettingsProvisioning.Infrastructure.DependencyInjection
{
    public class SettingsProvisioningRegistryCollection : ServiceRegistry
    {
        public SettingsProvisioningRegistryCollection()
        {
            Scan(
                scanner =>
                {
                    scanner.AssemblyContainingType<SettingsProvisioningRegistryCollection>();
                    scanner.WithDefaultConventions();
                });

            For<IFileSystem>().Use<FileSystem>().Singleton();
            For<ISettingsFactory>().Use<SettingsFactory>().Singleton();
            For<IDirectorySearchServant>().Use<DirectorySearchServant>().Singleton();
        }
    }
}