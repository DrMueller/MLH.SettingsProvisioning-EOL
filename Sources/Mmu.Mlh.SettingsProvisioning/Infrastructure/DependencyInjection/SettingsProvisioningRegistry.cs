using System.IO.Abstractions;
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
        }
    }
}