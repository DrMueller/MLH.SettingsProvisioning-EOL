using System.IO.Abstractions;
using Mmu.Mlh.ApplicationExtensions.Areas.Dropbox.Services;
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
        private Mock<IDirectorySearchServant> _directorySearchServantMock;
        private Mock<IFileSystem> _fileSystemMock;
        private Mock<IDropboxLocator> _dropboxLocatorMock;
        private SettingsFactory _sut;

        [SetUp]
        public void Align()
        {
            _directorySearchServantMock = new Mock<IDirectorySearchServant>();
            _fileSystemMock = new Mock<IFileSystem>();
            _dropboxLocatorMock = new Mock<IDropboxLocator>();

            _sut = new SettingsFactory(
                _directorySearchServantMock.Object,
                _dropboxLocatorMock.Object,
                _fileSystemMock.Object);
        }

        [Test]
        public void CreatingSettings_WithNotFoundAppSettings_Throws_OfTypeAppSettingsNotFoundException()
        {
            // Arrange
            const string BasePath = @"C:\Users\";
            _directorySearchServantMock.Setup(f => f.SearchAppSettings(It.IsAny<string>()))
                .Returns(new AppSettingsSearchResult(false, "tra"));
            var config = new SettingsConfiguration("Test", "Test1", BasePath);

            // Act & Assert
            Assert.Throws<AppSettingsNotFoundException>(() => _sut.CreateSettings<object>(config));
        }

        [Test]
        public void CreatingSettings_WithNotFoundAppSettings_Throws_WithbasePathAsMessage()
        {
            // Arrange
            const string BasePath = @"C:\Users\";
            _directorySearchServantMock.Setup(f => f.SearchAppSettings(It.IsAny<string>()))
                .Returns(new AppSettingsSearchResult(false, "tra"));
            var config = new SettingsConfiguration("Test", "Test1", BasePath);

            // Act & Assert
            Assert.That(
                () => _sut.CreateSettings<object>(config),
                Throws.TypeOf<AppSettingsNotFoundException>().With.Message.EqualTo(BasePath));
        }
    }
}