using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using RestfulQr.Api.Core.Errors;
using RestfulQr.Api.Core.Providers;
using RestfulQr.Api.Services.Impl;
using RestfulQr.Core.Rendering;
using RestfulQr.Core.Util;
using RestfulQr.Domain;
using RestfulQr.Domain.ViewModels;
using RestfulQr.Persistence;
using RestfulQr.Persistence.Local;
using Serilog;

namespace RestfulQr.Api.Controllers.Api.V1
{
    [ApiController]
    [Route("api/v1/qrcode")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Authorize]
    public class QrCodeController : ControllerBase
    {
        private readonly QrCodeRenderOptions renderOptions;
        private readonly IQrCodeRenderer renderer;
        private readonly ApiKeyProvider apiKeyProvider;
        private readonly IImageService imageService;
        private readonly IConfiguration config;

        public QrCodeController(QrCodeRenderOptions renderOptions, IQrCodeRenderer renderer, ApiKeyProvider apiKeyProvider, IImageService imageService, IConfiguration config)
        {
            this.renderOptions = renderOptions;
            this.renderer = renderer;
            this.apiKeyProvider = apiKeyProvider;
            this.imageService = imageService;
            this.config = config;
        }

        /// <summary>
        /// Encodes text into a QR code. The content of the QR codes is from a JSON object with a non-empty string property "content"
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /text
        ///     {
        ///        "content": "hi!"
        ///     }
        /// </remarks>
        /// <example> { "content": "text be encoded" } </example>
        /// <returns>
        /// The result of creation, including the location of the created file
        /// </returns>
        /// <response code="201">Returns the newly created qr code</response>
        /// <response code="400">The json was not found or invalid</response>
        /// <response code="500">The qr code not be created</response>   
        [HttpPost("text")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RestfulQrCode))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Text([FromBody] object body)
        {
            try
            {
                var json = JsonSerializer.Serialize(body);

                var jsonDocument = JsonDocument.Parse(json);

                if (!jsonDocument.RootElement.TryGetProperty("content", out JsonElement contentProperty))
                {
                   return BadRequest(StandardErrorMessages.Requests.Text.MustContainContent);
                }

                var content = contentProperty.GetString() ?? "";

                if (string.IsNullOrWhiteSpace(content)) 
                {
                    return BadRequest(StandardErrorMessages.Requests.Text.ContentMustNotBeEmpty);
                }

                var qrCode = await renderer.RenderAsync(QrCodeType.Text, content);

                if (qrCode == null) 
                {
                    throw new Exception(StandardErrorMessages.Requests.UnableToRenderQrCode);
                }

                return await HandleAsync(qrCode, QrCodeType.Text, content);
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to create a QR code");

                return StatusCode(500);
            }
        }

         /// <summary>
        /// Encodes a valid json object into a qr code
        /// </summary>
        /// <param name="body">The JSON oject to encode</param>
        /// <returns>
        /// The result of creation, including the location of the created file
        /// </returns>
        /// <response code="201">Returns the newly created qr code</response>
        /// <response code="400">The json was not found or invalid</response>
        /// <response code="500">The qr code not be created</response>   
        [HttpPost("json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Json([FromBody] object body)
        {
            try
            {
                var json = JsonSerializer.Serialize(body);

                var jsonDocument = JsonDocument.Parse(json);

                var content = jsonDocument.RootElement.ToString();

                if (string.IsNullOrEmpty(content) || jsonDocument.RootElement.EnumerateObject().Count() == 0)
                {
                    return BadRequest("JSON body as not or an invalid format");
                }

                var result = await renderer.RenderAsync(QrCodeType.Json, content);

                if (result == null) 
                {
                    throw new Exception(StandardErrorMessages.Requests.UnableToRenderQrCode);
                }

                return await HandleAsync(result, QrCodeType.Json, content);
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to create a QR code");

                return StatusCode(500);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">
        /// The information to encode into the qr code. 
        /// Specific to the Bookmark type qr code.
        /// </param>
        /// <returns>
        /// The result of creation, including the location of the created file
        /// </returns>
        /// <response code="201">Returns the newly created qr code</response>
        /// <response code="400">The json was not found or invalid</response>
        /// <response code="500">The qr code not be created</response>   
        [HttpPost("bookmark")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RestfulQrCode))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Bookmark([FromBody] CreateBookmarkModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelErrors(ModelState));
            }

            try
            {
                var result = await renderer.RenderAsync(QrCodeType.Bookmark, model);

                if (result == null) 
                {
                    throw new Exception(StandardErrorMessages.Requests.UnableToRenderQrCode);
                }

                return await HandleAsync(result, QrCodeType.Bookmark, model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">
        /// The information to encode into the qr code. 
        /// Specific to the calendar event type qr code.
        /// </param>
        /// <returns>
        /// The result of creation, including the location of the created file
        /// </returns>
        /// <response code="201">Returns the newly created qr code</response>
        /// <response code="400">The json was not found or invalid</response>
        /// <response code="500">The qr code not be created</response>   
        [HttpPost("calendar")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Calendar(CreateCalendarEventModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelErrors(ModelState));
            }

            try
            {
                var result = await renderer.RenderAsync(QrCodeType.CalendarEvent, model);

                if (result == null) 
                {
                    throw new Exception(StandardErrorMessages.Requests.UnableToRenderQrCode);
                }

                return await HandleAsync(result, QrCodeType.CalendarEvent, model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// The result of creation, including the location of the created file
        /// </returns>
        /// <response code="201">Returns the newly created qr code</response>
        /// <response code="400">The json was not found or invalid</response>
        /// <response code="500">The qr code not be created</response>   
        [HttpPost("contact")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ContactData(CreateContactDataEventModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelErrors(ModelState));
            }

            try
            {
                var result = await renderer.RenderAsync(QrCodeType.Contact, model);
                
                if (result == null) 
                {
                    throw new Exception(StandardErrorMessages.Requests.UnableToRenderQrCode);
                }

                return await HandleAsync(result, QrCodeType.Contact, model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// The result of creation, including the location of the created file
        /// </returns>
        /// <response code="201">Returns the newly created qr code</response>
        /// <response code="400">The json was not found or invalid</response>
        /// <response code="500">The qr code not be created</response>   
        [HttpPost("geolocation")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Geolocation(CreateGeoLocationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelErrors(ModelState));
            }

            try
            {
                var result = await renderer.RenderAsync(QrCodeType.Geolocation, model);

                if (result == null) 
                {
                    throw new Exception(StandardErrorMessages.Requests.UnableToRenderQrCode);
                }

                return await HandleAsync(result, QrCodeType.Geolocation, model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// The result of creation, including the location of the created file
        /// </returns>
        /// <response code="201">Returns the newly created qr code</response>
        /// <response code="400">The json was not found or invalid</response>
        /// <response code="500">The qr code not be created</response>   
        [HttpPost("email")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Email(CreateEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelErrors(ModelState));
            }

            try
            {
                var result = await renderer.RenderAsync(QrCodeType.Email, model);

                if (result == null) 
                {
                    throw new Exception(StandardErrorMessages.Requests.UnableToRenderQrCode);
                }

                return await HandleAsync(result, QrCodeType.Email, model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// The result of creation, including the location of the created file
        /// </returns>
        /// <response code="201">Returns the newly created qr code</response>
        /// <response code="400">The json was not found or invalid</response>
        /// <response code="500">The qr code not be created</response>   
        [HttpPost("message")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TextMessage(CreateTextMessageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelErrors(ModelState));
            }

            try
            {
                var result = await renderer.RenderAsync(QrCodeType.TextMessage, model);

                if (result == null) 
                {
                    throw new Exception(StandardErrorMessages.Requests.UnableToRenderQrCode);
                }

                return await HandleAsync(result, QrCodeType.TextMessage, model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// The result of creation, including the location of the created file
        /// </returns>
        /// <response code="201">Returns the newly created qr code</response>
        /// <response code="400">The json was not found or invalid</response>
        /// <response code="500">The qr code not be created</response>   
        [HttpPost("website")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Website(CreateUrlModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelErrors(ModelState));
            }

            try
            {
                var result = await renderer.RenderAsync(QrCodeType.Url, model);

                if (result == null) 
                {
                    throw new Exception(StandardErrorMessages.Requests.UnableToRenderQrCode);
                }

                return await HandleAsync(result, QrCodeType.Url, model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// The result of creation, including the location of the created file
        /// </returns>
        /// <response code="201">Returns the newly created qr code</response>
        /// <response code="400">The json was not found or invalid</response>
        /// <response code="500">The qr code not be created</response>   
        [HttpPost("whatsapp")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> WhatsApp(CreateWhatsAppMessageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelErrors(ModelState));
            }

            try
            {
                var result = await renderer.RenderAsync(QrCodeType.WhatsApp, model);

                if (result == null) 
                {
                    throw new Exception(StandardErrorMessages.Requests.UnableToRenderQrCode);
                }

                return await HandleAsync(result, QrCodeType.WhatsApp, model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// The result of creation, including the location of the created file
        /// </returns>
        /// <response code="201">Returns the newly created qr code</response>
        /// <response code="400">The json was not found or invalid</response>
        /// <response code="500">The qr code not be created</response>   
        [HttpPost("wifi")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Wifi(CreateWifiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelErrors(ModelState));
            }

            try
            {
                var result = await renderer.RenderAsync(QrCodeType.Wifi, model);

                if (result == null) 
                {
                    throw new Exception(StandardErrorMessages.Requests.UnableToRenderQrCode);
                }

                return await HandleAsync(result, QrCodeType.Wifi, model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

                return StatusCode(500);
            }
        }

        private async Task<IActionResult> HandleAsync(byte[] result, QrCodeType qrCodeType, object model) 
        {
            // Generate a file name for the qr code
            var id = Guid.NewGuid();

            var filename = $"{id}.{ImageUtils.GetFileExtensionFromImageType(renderOptions.RenderType)}";
            var host = config["HostUrl"] ?? $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            // Generate a database result for the file
            var qrCode = new RestfulQrCode
            {
                Id = id,
                Created = DateTime.Now.ToUniversalTime(),
                CreatedBy = apiKeyProvider.ApiKey.Id,
                Filename = filename,
                Model = model,
                RenderType = renderOptions.RenderType,
                Type = qrCodeType,
                PublicUrl = string.Join(@"/", host, "images", apiKeyProvider.ApiKey.LocationId, filename)
            };

            // Save the file in S3
            var s3Result = await imageService.UploadAsync(apiKeyProvider.ApiKey, filename, result);

            if (!s3Result)
            {
                return StatusCode(500);
            }

            // Save the db
            var saveResult = await imageService.CreateAsync(qrCode);

            if (saveResult == null)
            {
                return StatusCode(500);
            }

            return Created(qrCode.PublicUrl, qrCode);
        }

        private string[] GetModelErrors(ModelStateDictionary state)
        {
            var errors = state.Select(x => x.Value?.Errors);

            if (!errors.Any())
            {
                return Array.Empty<string>();
            }
            
            return errors
                .Select(x => x!.First().ErrorMessage)
                .ToArray();
        }
    }
}