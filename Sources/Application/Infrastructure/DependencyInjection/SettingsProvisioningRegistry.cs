using System.IO.Abstractions;
using Mmu.Mlh.SettingsProvisioning.Areas.Factories;
using Mmu.Mlh.SettingsProvisioning.Areas.Factories.Implementation;
using Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants;
using Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants.Implementation;
using StructureMap;

namespace Mmu.Mlh.SettingsProvisioning.Infrastructure.DependencyInjection
{
    public class SettingsProvisioningRegistry : Registry
    {
        public SettingsProvisioningRegistry()
        {
            Scan(
                scanner =>
                {
                    scanner.AssemblyContainingType<SettingsProvisioningRegistry>();
                    scanner.WithDefaultConventions();
                });

            For<IFileSystem>().Use<FileSystem>();
            For<ISettingsFactory>().Use<SettingsFactory>();
            For<ISectionConfigurationServant>().Use<SectionConfigurationServant>();
            For<IDirectorySearchServant>().Use<DirectorySearchServant>();
            For<IConfigurationRootFactory>().Use<ConfigurationRootFactory>();
        }
    }
}