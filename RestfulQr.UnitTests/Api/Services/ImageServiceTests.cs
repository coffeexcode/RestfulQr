using Moq;
using NUnit.Framework;
using RestfulQr.Api.Core.Caching;
using RestfulQr.Api.Services;
using RestfulQr.Api.Services.Impl;
using RestfulQr.Domain;
using RestfulQr.Persistence;
using RestfulQr.Persistence.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Api.Services
{
    [TestFixture, Category("unit")]
    public class ImageServiceTests
    {
        private ApiKey apiKey = GetApiKey();
        private byte[] imageData = new byte[1024];

        private long defaultLocationId = 1;
        private string defaultExtension = "png";

        private Mock<ICacheProvider<byte[]>> imageCacheMock;
        private Mock<IImagePersistor> imagePersistorMock;
        private Mock<IRestfulQrCodeRepository> restfulQrCodeRepositoryMock;
        
        [SetUp]
        public void Setup()
        {
            imageCacheMock = new Mock<ICacheProvider<byte[]>>(MockBehavior.Strict);
            imagePersistorMock = new Mock<IImagePersistor>(MockBehavior.Strict);
            restfulQrCodeRepositoryMock = new Mock<IRestfulQrCodeRepository>(MockBehavior.Strict);

            imagePersistorMock
                .Setup(x => x.UploadAsync(It.IsAny<ApiKey>(), It.IsAny<string>(), It.IsAny<byte[]>()))
                .ReturnsAsync(true);
            imagePersistorMock
                .Setup(x => x.GetImageAsync(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(imageData);

            imageCacheMock
                .Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Returns(Task.CompletedTask);
            imageCacheMock
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<byte[]>(null));
        }

        [Test]
        public async Task CanUploadToS3()
        {
            // Arrange
            var filename = $"{Guid.NewGuid()}.{defaultExtension}";
            var path = string.Join(@"/", defaultLocationId, filename);
            var service = GetImageService();

            // Act
            var result = await service.UploadAsync(apiKey, filename, imageData);

            // Assert
            imageCacheMock.Verify(x => x.SetAsync(path, imageData));
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase(1, "png", true)]
        [TestCase(4, "gif", false)]
        [TestCase(-1, "png", false)]
        [TestCase(42, "svg", true)]
        public void CanValidateFilePaths(long locationId, string extension, bool isValid)
        {
            // Arrange
            var filename = $"{Guid.NewGuid()}.{extension}";
            var service = GetImageService();

            // Act
            var result = service.IsValid(locationId, filename);

            // Assert
            Assert.AreEqual(isValid, result);
        }

        [Test]
        public async Task CanRetrieveImage_FromS3_AndWillCache()
        {
            // Arrange
            var filename = $"{Guid.NewGuid()}.{defaultExtension}";
            var service = GetImageService();

            // Act
            var result = await service.GetImageAsync(defaultLocationId, filename);

            // Assert
            imageCacheMock.Verify(x => x.SetAsync(It.IsAny<string>(), imageData));
            imagePersistorMock.Verify(x => x.GetImageAsync(defaultLocationId, filename));
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task CanRetrieveImage_FromCache()
        {
            // Arrange
            imageCacheMock
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(imageData);

            var filename = $"{Guid.NewGuid()}.{defaultExtension}";
            var service = GetImageService();

            // Act
            var result = await service.GetImageAsync(defaultLocationId, filename);

            // Assert
            imageCacheMock.Verify(x => x.GetAsync(It.IsAny<string>()));
            imagePersistorMock.Verify(x => x.GetImageAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Never);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task WillCache1Byte_WhenImageNotFound()
        {
            // Arrange
            imagePersistorMock
                .Setup(x => x.GetImageAsync(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(Task.FromResult<byte[]>(null));

            var filename = $"{Guid.NewGuid()}.{defaultExtension}";
            var service = GetImageService();

            // Act
            var result = await service.GetImageAsync(defaultLocationId, filename);

            // Assert
            imageCacheMock.Verify(x => x.GetAsync(It.IsAny<string>()));
            imagePersistorMock.Verify(x => x.GetImageAsync(It.IsAny<long>(), It.IsAny<string>()));
            imageCacheMock.Verify(x => x.SetAsync(It.IsAny<string>(), new byte[1] { 0 }));
            Assert.IsNull(result);
        }

        [Test]
        public async Task WillReturnNull_WhenCacheHits1Byte()
        {
            // Arrange
            imageCacheMock
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new byte[1]);

            var filename = $"{Guid.NewGuid()}.{defaultExtension}";
            var service = GetImageService();

            // Act
            var result = await service.GetImageAsync(defaultLocationId, filename);

            // Assert
            imageCacheMock.Verify(x => x.GetAsync(It.IsAny<string>()));
            imagePersistorMock.Verify(x => x.GetImageAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Never);
            imageCacheMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Never);
            Assert.IsNull(result);
        }

        private ImageService GetImageService()
        {
            return new ImageService(
                imageCacheMock.Object,
                imagePersistorMock.Object,
                restfulQrCodeRepositoryMock.Object
            );
        }

        private static ApiKey GetApiKey()
        {
            return new ApiKey()
            {
                Created = DateTime.Now,
                Id = Guid.NewGuid(),
                LastUsed = DateTime.Now,
                LocationId = 1
            };
        }
    }
}
