using Microsoft.Extensions.Configuration;
using Mmu.Mlh.SettingsProvisioning.Areas.Factories.Implementation;
using Mmu.Mlh.SettingsProvisioning.Areas.Factories.Servants;
using Mmu.Mlh.SettingsProvisioning.Areas.Models;
using Mmu.Mlh.SettingsProvisioning.Infrastructure.Exceptions;
using Moq;
using NUnit.Framework;

namespace Mmu.Mlh.SettingsProvisioning.UnitTests.TestingAreas.Areas.Factories
{
    [TestFixture]
    public class SettingsFactoryUnitTests
    {
        private Mock<IConfigurationRootFactory> _configurationRootFactoryMock;
        private Mock<IDirectorySearchServant> _directorySearchServantMock;
        private Mock<ISectionConfigurationServant> _sectionConfigurationServantMock;
        private SettingsFactory _sut;

        [SetUp]
        public void Align()
        {
            _directorySearchServantMock = new Mock<IDirectorySearchServant>();
            _configurationRootFactoryMock = new Mock<IConfigurationRootFactory>();
            _sectionConfigurationServantMock = new Mock<ISectionConfigurationServant>();

            _sut = new SettingsFactory(
                _directorySearchServantMock.Object,
                _configurationRootFactoryMock.Object,
                _sectionConfigurationServantMock.Object);
        }

        [Test]
        public void CreatingSettings_Calls_ConfigurationRootFactory_WithReturnedAppSettingsPath()
        {
            // Arrange
            const string SettingsPath = @"C:\Users\appsettings.json";
            _directorySearchServantMock.Setup(f => f.SearchAppSettings(It.IsAny<string>()))
                .Returns(new AppSettingsSearchResult(true, SettingsPath));

            // Act
            _sut.CreateSettings<object>("Test", "Test1", string.Empty);

            // Assert
            _configurationRootFactoryMock.Verify(f => f.Create(SettingsPath, It.IsAny<string>()));
        }

        [Test]
        public void CreatingSettings_Calls_SectionConfigurationServant_WithPassedSectionKey()
        {
            // Arrange
            const string SectionKey = "Tra";
            _directorySearchServantMock.Setup(f => f.SearchAppSettings(It.IsAny<string>()))
                .Returns(new AppSettingsSearchResult(true, "tra"));

            // Act
            _sut.CreateSettings<object>(SectionKey, string.Empty, string.Empty);

            // Assert
            _sectionConfigurationServantMock.Verify(f => f.ConfigureFromSection<object>(
                It.IsAny<IConfigurationRoot>(),
                SectionKey));
        }

        [Test]
        public void CreatingSettings_Calls_DirectorySearchServant_WithPassedBasePath()
        {
            // Arrange
            const string BasePath = @"C:\Users\";
            _directorySearchServantMock.Setup(f => f.SearchAppSettings(It.IsAny<string>()))
                .Returns(new AppSettingsSearchResult(true, "tra"));

            // Act
            _sut.CreateSettings<object>("Test", "Test1", BasePath);

            // Assert
            _directorySearchServantMock.Verify(f => f.SearchAppSettings(BasePath));
        }

        [Test]
        public void CreatingSettings_WithNotFoundAppSettings_Throws_OfTypeAppSettingsNotFoundException()
        {
            // Arrange
            const string BasePath = @"C:\Users\";
            _directorySearchServantMock.Setup(f => f.SearchAppSettings(It.IsAny<string>()))
                .Returns(new AppSettingsSearchResult(false, "tra"));

            // Act & Assert
            Assert.Throws<AppSettingsNotFoundException>(() => _sut.CreateSettings<object>("Test", "Test1", BasePath));
        }

        [Test]
        public void CreatingSettings_WithNotFoundAppSettings_Throws_WithbasePathAsMessage()
        {
            // Arrange
            const string BasePath = @"C:\Users\";
            _directorySearchServantMock.Setup(f => f.SearchAppSettings(It.IsAny<string>()))
                .Returns(new AppSettingsSearchResult(false, "tra"));

            // Act & Assert
            Assert.That(
                () => _sut.CreateSettings<object>("Test", "Test1", BasePath),
                Throws.TypeOf<AppSettingsNotFoundException>().With.Message.EqualTo(BasePath));
        }
    }
}