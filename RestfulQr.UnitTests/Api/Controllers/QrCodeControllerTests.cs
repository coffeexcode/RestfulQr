using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using RestfulQr.Api.Controllers.Api.V1;
using RestfulQr.Api.Core.Providers;
using RestfulQr.Api.Services.Impl;
using RestfulQr.Core.Rendering;
using RestfulQr.Domain;
using RestfulQr.Domain.ViewModels;
using RestfulQr.Persistence;
using RestfulQr.Persistence.Local;
using RestfulQr.UnitTests.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Api.Controllers
{
    [TestFixture, Category("unit")]
    public class QrCodeControllerTests
    {
        private readonly string hostUrl = "https://host.com";
        private static byte[] defaultSuccessReturnData = new byte[1024];

        private Mock<IQrCodeRenderer> qrCodeRendererMock;
        private Mock<IImageService> imageServiceMock;

        [SetUp]
        public void Setup()
        {
            qrCodeRendererMock = new Mock<IQrCodeRenderer>(MockBehavior.Strict);
            imageServiceMock = new Mock<IImageService>(MockBehavior.Strict);

            imageServiceMock
                .Setup(x => x.UploadAsync(It.IsAny<ApiKey>(), It.IsAny<string>(), It.IsAny<byte[]>()))
                .ReturnsAsync(true);
            imageServiceMock
                .Setup(x => x.CreateAsync(It.IsAny<RestfulQrCode>()))
                .ReturnsAsync(new RestfulQrCode());
        }

        [Test]
        public async Task CreateText_Invalid()
        {
            // Setup
            var controller = CreateController();
            var model = new MockTextModel().Invalid();
            RendererShouldReturn<object>();

            // Perform
            var response = await controller.Text(model);

            // Assert
            AssertIsBadRequest(response);
        }

        [Test]
        public async Task CreateText_Valid()
        {
            // Setup
            var controller = CreateController();
            var model = new MockTextModel().Valid();
            RendererShouldReturn<object>(defaultSuccessReturnData);

            // Perform
            var response = await controller.Text(model);

            // Assert
            AssertIsOk(response);
        }

        [Test]
        public async Task CreateJson_Invalid()
        {
            // Setup
            var controller = CreateController();
            var model = new MockJsonModel().Invalid();
            RendererShouldReturn<object>();

            // Perform
            var response = await controller.Json(model);

            // Assert
            AssertIsBadRequest(response);
        }

        [Test]
        public async Task CreateJson_Valid()
        {
            // Setup
            var controller = CreateController();
            var model = new MockJsonModel().Valid();
            RendererShouldReturn<object>(defaultSuccessReturnData);

            // Perform
            var response = await controller.Json(model);

            // Assert
            AssertIsOk(response);
        }

        [Test]
        public async Task CreateBookmark_Invalid()
        {
            // Setup
            var controller = CreateController(modelStateInvalid: true);
            var model = new MockBookmarkModel().Invalid();
            RendererShouldReturn<CreateBookmarkModel>();

            // Perform
            var response = await controller.Bookmark(model);

            // Assert
            AssertIsBadRequest(response);
        }

        [Test]
        public async Task CreateBookmark_Valid()
        {
            // Ararnge
            var controller = CreateController();
            var model = new MockBookmarkModel().Valid();
            RendererShouldReturn<CreateBookmarkModel>(defaultSuccessReturnData);

            // Act
            var response = await controller.Bookmark(model);

            // Assert
            AssertIsOk(response);
        }

        [Test]
        public async Task CreateCalendarEvent_Invalid()
        {
            // Setup
            var controller = CreateController(modelStateInvalid: true);
            var model = new MockCalendarEventModel().Invalid();
            RendererShouldReturn<CreateCalendarEventModel>();

            // Perform
            var response = await controller.Calendar(model);

            // Assert
            AssertIsBadRequest(response);
        }

        [Test]
        public async Task CreateCalendarEvent_Valid()
        {
            // Ararnge
            var controller = CreateController();
            var model = new MockCalendarEventModel().Valid();
            RendererShouldReturn<CreateCalendarEventModel>(defaultSuccessReturnData);

            // Act
            var response = await controller.Calendar(model);

            // Assert
            AssertIsOk(response);
        }

        [Test]
        public async Task CreateContactDataEvent_Invalid()
        {
            // Setup
            var controller = CreateController(modelStateInvalid: true);
            var model = new MockContactDataEventModel().Invalid();
            RendererShouldReturn<CreateContactDataEventModel>();

            // Perform
            var response = await controller.ContactData(model);

            // Assert
            AssertIsBadRequest(response);
        }

        [Test]
        public async Task CreateContactDataEvent_Valid()
        {
            // Ararnge
            var controller = CreateController();
            var model = new MockContactDataEventModel().Valid();
            RendererShouldReturn<CreateContactDataEventModel>(defaultSuccessReturnData);

            // Act
            var response = await controller.ContactData(model);

            // Assert
            AssertIsOk(response);
        }

        [Test]
        public async Task CreateEmail_Invalid()
        {
            // Setup
            var controller = CreateController(modelStateInvalid: true);
            var model = new MockEmailModel().Invalid();
            RendererShouldReturn<CreateEmailModel>();

            // Perform
            var response = await controller.Email(model);

            // Assert
            AssertIsBadRequest(response);
        }

        [Test]
        public async Task CreateEmail_Valid()
        {
            // Ararnge
            var controller = CreateController();
            var model = new MockEmailModel().Valid();
            RendererShouldReturn<CreateEmailModel>(defaultSuccessReturnData);

            // Act
            var response = await controller.Email(model);

            // Assert
            AssertIsOk(response);
        }

        [Test]
        public async Task CreateGeoLocation_Invalid()
        {
            // Setup
            var controller = CreateController(modelStateInvalid: true);
            var model = new MockGeoLocationModel().Invalid();
            RendererShouldReturn<CreateGeoLocationModel>();

            // Perform
            var response = await controller.Geolocation(model);

            // Assert
            AssertIsBadRequest(response);
        }

        [Test]
        public async Task CreateGeoLocation_Valid()
        {
            // Ararnge
            var controller = CreateController();
            var model = new MockGeoLocationModel().Valid();
            RendererShouldReturn<CreateGeoLocationModel>(defaultSuccessReturnData);

            // Act
            var response = await controller.Geolocation(model);

            // Assert
            AssertIsOk(response);
        }

        [Test]
        public async Task CreateTextMessage_Invalid()
        {
            // Setup
            var controller = CreateController(modelStateInvalid: true);
            var model = new MockTextMessageModel().Invalid();
            RendererShouldReturn<CreateTextMessageModel>();

            // Perform
            var response = await controller.TextMessage(model);

            // Assert
            AssertIsBadRequest(response);
        }

        [Test]
        public async Task CreateTextMessage_Valid()
        {
            // Ararnge
            var controller = CreateController();
            var model = new MockTextMessageModel().Valid();
            RendererShouldReturn<CreateTextMessageModel>(defaultSuccessReturnData);

            // Act
            var response = await controller.TextMessage(model);

            // Assert
            AssertIsOk(response);
        }

        [Test]
        public async Task CreateUrl_Invalid()
        {
            // Setup
            var controller = CreateController(modelStateInvalid: true);
            var model = new MockUrlModel().Invalid();
            RendererShouldReturn<CreateUrlModel>();

            // Perform
            var response = await controller.Website(model);

            // Assert
            AssertIsBadRequest(response);
        }

        [Test]
        public async Task CreateUrl_Valid()
        {
            // Ararnge
            var controller = CreateController();
            var model = new MockUrlModel().Valid();
            RendererShouldReturn<CreateUrlModel>(defaultSuccessReturnData);

            // Act
            var response = await controller.Website(model);

            // Assert
            AssertIsOk(response);
        }

        [Test]
        public async Task CreateWhatsAppMessage_Invalid()
        {
            // Setup
            var controller = CreateController(modelStateInvalid: true);
            var model = new MockWhatsAppMessageModel().Invalid();
            RendererShouldReturn<CreateWhatsAppMessageModel>();

            // Perform
            var response = await controller.WhatsApp(model);

            // Assert
            AssertIsBadRequest(response);
        }

        [Test]
        public async Task CreateWhatsAppMessage_Valid()
        {
            // Ararnge
            var controller = CreateController();
            var model = new MockWhatsAppMessageModel().Valid();
            RendererShouldReturn<CreateWhatsAppMessageModel>(defaultSuccessReturnData);

            // Act
            var response = await controller.WhatsApp(model);

            // Assert
            AssertIsOk(response);
        }

        [Test]
        public async Task CreateWifi_Invalid()
        {
            // Setup
            var controller = CreateController(modelStateInvalid: true);
            var model = new MockWifiModel().Invalid();
            RendererShouldReturn<CreateWifiModel>();

            // Perform
            var response = await controller.Wifi(model);

            // Assert
            AssertIsBadRequest(response);
        }

        [Test]
        public async Task CreateWifi_Valid()
        {
            // Ararnge
            var controller = CreateController();
            var model = new MockWifiModel().Valid();
            RendererShouldReturn<CreateWifiModel>(defaultSuccessReturnData);

            // Act
            var response = await controller.Wifi(model);

            // Assert
            AssertIsOk(response);
        }

        [TestCaseSource("ControllerShouldThrow500Cases")]
        public async Task Controller_Throws500_WhenCannotRenderQrCode(object instance, string typeOverride = null)
        {
            // Arrange
            qrCodeRendererMock
                .Setup(x => x.RenderAsync(It.IsAny<QrCodeType>(), It.IsAny<object>()))
                .Returns(Task.FromResult<byte[]>(null));
            var controller = CreateController();

            // Act
            var response = await GetResponseByType(controller, instance, typeOverride);

            if (response == null)
            {
                throw new Exception("Unable to get any response");
            }

            // Assert
            AssertIsServerError(response);
        }

        [TestCaseSource("ControllerShouldThrow500Cases")]
        public async Task Controller_Throws500_WhenCannotUploadToS3(object instance, string typeOverride = null)
        {
            // Arrange
            qrCodeRendererMock
                .Setup(x => x.RenderAsync(It.IsAny<QrCodeType>(), It.IsAny<object>()))
                .ReturnsAsync(defaultSuccessReturnData);
            imageServiceMock
                .Setup(x => x.UploadAsync(It.IsAny<ApiKey>(), It.IsAny<string>(), It.IsAny<byte[]>()))
                .ReturnsAsync(null);

            var controller = CreateController();

            // Act
            var response = await GetResponseByType(controller, instance, typeOverride);

            if (response == null)
            {
                throw new Exception("Unable to get any response");
            }

            // Assert
            AssertIsServerError(response);
            imageServiceMock.Verify(x => x.UploadAsync(It.IsAny<ApiKey>(), It.IsAny<string>(), It.IsAny<byte[]>()));
        }

        [TestCaseSource("ControllerShouldThrow500Cases")]
        public async Task Controller_Throws500_WhenCannotSaveToDatabase(object instance, string typeOverride = null)
        {
            // Arrange
            qrCodeRendererMock
                .Setup(x => x.RenderAsync(It.IsAny<QrCodeType>(), It.IsAny<object>()))
                .ReturnsAsync(defaultSuccessReturnData);
            imageServiceMock
                .Setup(x => x.UploadAsync(It.IsAny<ApiKey>(), It.IsAny<string>(), It.IsAny<byte[]>()))
                .ReturnsAsync(true);
            imageServiceMock
                .Setup(x => x.CreateAsync(It.IsAny<RestfulQrCode>()))
                .Returns(Task.FromResult<RestfulQrCode>(null));

            var controller = CreateController();

            // Act
            var response = await GetResponseByType(controller, instance, typeOverride);

            if (response == null)
            {
                throw new Exception("Unable to get any response");
            }

            // Assert
            AssertIsServerError(response);
            imageServiceMock.Verify(x => x.UploadAsync(It.IsAny<ApiKey>(), It.IsAny<string>(), It.IsAny<byte[]>()));
            imageServiceMock.Verify(x => x.CreateAsync(It.IsAny<RestfulQrCode>()));
        }

        private async Task<IActionResult> GetResponseByType(QrCodeController controller, object instance, string typeOverride)
        {
            var response = instance switch
            {
                CreateBookmarkModel => await controller.Bookmark(instance as CreateBookmarkModel),
                CreateCalendarEventModel => await controller.Calendar(instance as CreateCalendarEventModel),
                CreateContactDataEventModel => await controller.ContactData(instance as CreateContactDataEventModel),
                CreateEmailModel => await controller.Email(instance as CreateEmailModel),
                CreateGeoLocationModel => await controller.Geolocation(instance as CreateGeoLocationModel),
                CreateTextMessageModel => await controller.TextMessage(instance as CreateTextMessageModel),
                CreateWhatsAppMessageModel => await controller.WhatsApp(instance as CreateWhatsAppMessageModel),
                CreateUrlModel => await controller.Website(instance as CreateUrlModel),
                CreateWifiModel => await controller.Wifi(instance as CreateWifiModel),
                _ => null
            };

            if (typeOverride != null)
            {
                if (typeOverride == "text")
                {
                    response = await controller.Text(instance);
                }
                else if (typeOverride == "json")
                {
                    response = await controller.Json(instance);
                }
            }

            return response;
        }

        private static IEnumerable<TestCaseData> ControllerShouldThrow500Cases
        {
            get
            {
                yield return new TestCaseData(new MockBookmarkModel().Valid(), null);
                yield return new TestCaseData(new MockCalendarEventModel().Valid(), null);
                yield return new TestCaseData(new MockContactDataEventModel().Valid(), null);
                yield return new TestCaseData(new MockEmailModel().Valid(), null);
                yield return new TestCaseData(new MockGeoLocationModel().Valid(), null);
                yield return new TestCaseData(new MockTextMessageModel().Valid(), null);
                yield return new TestCaseData(new MockUrlModel().Valid(), null);
                yield return new TestCaseData(new MockWhatsAppMessageModel().Valid(), null);
                yield return new TestCaseData(new MockWifiModel().Valid(), null);
                yield return new TestCaseData(new MockTextModel().Valid(), "text");
                yield return new TestCaseData(new MockJsonModel().Valid(), "json");
            }
        }

        private QrCodeController CreateController(bool modelStateInvalid = false)
        {
            var renderOptions = CreateQrCodeRenderOptions();
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Host = new HostString("localhost");
            httpContext.Request.Scheme = "http";

            var controller = new QrCodeController(
                renderOptions,
                qrCodeRendererMock.Object,
                CreateApiKeyProvider(),
                imageServiceMock.Object,
                GetConfiguration()
            )
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            if (modelStateInvalid)
            {
                controller.ModelState.AddModelError("key", "modelError");
            }

            return controller;
        }

        private QrCodeRenderOptions CreateQrCodeRenderOptions()
        {
            return new QrCodeRenderOptions
            {
                DarkColour = "#000000",
                LightColour = "#FFFFFF",
                RenderType = RenderType.Png,
                DrawQuietZones = true,
                EccLevel = QRCoder.QRCodeGenerator.ECCLevel.L,
                PixelsPerModule = 20,
                Preview = false
            };
        }

        private ApiKeyProvider CreateApiKeyProvider()
        {
            return new ApiKeyProvider()
            {
                ApiKey = new ApiKey
                {
                    Created = DateTime.MinValue,
                    Id = Guid.NewGuid(),
                    LastUsed = DateTime.Now,
                    LocationId = 1
                }
            };
        }

        private void RendererShouldReturn<T>(byte[] value = null)
        {
            qrCodeRendererMock
                .Setup(x => x.RenderAsync(It.IsAny<QrCodeType>(), It.IsAny<T>()))
                .ReturnsAsync(value);
        }

        private IConfiguration GetConfiguration()
        {
            var mock = new Mock<IConfiguration>(MockBehavior.Strict);

            mock.SetupGet(x => x["HostUrl"]).Returns(hostUrl);

            return mock.Object;
        }

        private void AssertIsOk(IActionResult response) => AssertAll<CreatedResult>(response, StatusCodes.Status201Created);

        private void AssertIsBadRequest(IActionResult response) => AssertAll<BadRequestObjectResult>(response, StatusCodes.Status400BadRequest);

        private void AssertIsServerError(IActionResult response) => AssertAll<StatusCodeResult>(response, StatusCodes.Status500InternalServerError);

        private void AssertAll<T>(IActionResult response, int statusCode) where T : class, IActionResult
        {
            // Type cast
            var typedResponse = (T)response;

            // Assert
            Assert.IsInstanceOf<IActionResult>(response);
            Assert.AreEqual(statusCode, (int)typedResponse.GetType().GetProperty("StatusCode").GetValue(typedResponse));
        }
    }
}
