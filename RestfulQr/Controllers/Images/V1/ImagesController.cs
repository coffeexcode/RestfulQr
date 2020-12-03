using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfulQr.Core;
using RestfulQr.Core.Exceptions;
using RestfulQr.Services;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RestfulQr.Controllers.Images.V1
{
    [ApiController]
    [Route("images")]
    [Produces("application/json", "image/png")]
    [Consumes("application/json")]
    public class ImagesController : ControllerBase
    {
        private readonly Services.IAuthorizationService authorizationService;
        private readonly IImageFileService imageFileService;
        private readonly IQrCodeService qrCodeService;
        private readonly ApiKeyProvider apiKeyProvider;

        public ImagesController(
            Services.IAuthorizationService authorizationService,
            IImageFileService imageFileService,
            IQrCodeService qrCodeService,
            ApiKeyProvider apiKeyProvider)
        {
            this.authorizationService = authorizationService;
            this.imageFileService = imageFileService;
            this.qrCodeService = qrCodeService;
            this.apiKeyProvider = apiKeyProvider;
        }

        /// <summary>
        /// Gets a generated qr code image from the filesystem
        /// </summary>
        /// <param name="filename">The name of the file to get</param>
        /// <returns>
        /// The image file if it exists and the user is authorized to access it.
        /// </returns>
        /// <response code="200">Returns the newly created qr code</response>
        /// <response code="400">The filename was not provided</response>
        /// <response code="401">A valid api key was not found in the X-Api-Key header</response>
        /// <response code="403">A valid api key was provided, but unable to access the file</response>
        /// <response code="404">A valid api key was provided,cannot find the file in the filesystem</response>
        /// <response code="500">The qr code not be created</response>   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{filename}")]
        public async Task<IActionResult> Get(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return BadRequest("Filename is required");
            }

            try
            {
                var canAccess = await authorizationService.CanAccessAsync(apiKeyProvider.ApiKey, filename);

                if (!canAccess)
                {
                    Log.Warning("API Key {key} was used to try an access a valid image file that it does not own", new { key = apiKeyProvider.ApiKey });
                    return Forbid();
                }

                // Access is allowed
                var bytes = await imageFileService.GetFileAsBytesAsync(filename);

                var extension = ExtractExtensionFromFilename(filename);

                return File(bytes, ImageUtil.GetMimeTypeByExtension(extension));
            }
            catch (EntryNotFoundException e)
            {
                Log.Information(e, "Attempted to delete a created qr code that was not found");
                return NotFound();
            }
            catch (FileNotFoundException e)
            {
                Log.Information(e, "Attemped to delete a file that was not found");
                return NotFound();
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while getting a qr code image with exception: {e}");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Deletes a qr code image from the filesystem and the backing store
        /// </summary>
        /// <param name="filename">The name of the file to delete</param>
        /// <returns>
        /// </returns>
        /// <response code="200">The file has been deleted</response>
        /// <response code="400">The filename was not provided</response>
        /// <response code="401">A valid api key was not found in the X-Api-Key header</response>
        /// <response code="403">A valid api key was provided, but unable to access the file</response>
        /// <response code="404">A valid api key was provided,cannot find the file in the filesystem</response>
        /// <response code="500">The qr code not be deleted</response>   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{filename}")]
        public async Task<IActionResult> Delete(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return BadRequest("Filename is required");
            }

            try
            {
                var canAccess = await authorizationService.CanAccessAsync(apiKeyProvider.ApiKey, filename);

                if (!canAccess)
                {
                    Log.Warning("API Key {key} was used to try an access a valid image file that it does not own", new { key = apiKeyProvider.ApiKey });
                    return Forbid();
                }

                // Access is allowed
                // Delete both the file and the entry in the backing store
                await qrCodeService.DeleteAsync(filename);

                return Ok();
            }
            catch (EntryNotFoundException e)
            {
                Log.Information(e, "Attempted to delete a created qr code that was not found");
                return NotFound();
            }
            catch (FileNotFoundException e)
            {
                Log.Information(e, "Attemped to delete a file that was not found");
                return NotFound();
            }
            catch (Exception e)
            {
                Log.Error(e, "Unable to delete qr code");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Extract the extension from a filename
        /// </summary>
        /// <param name="filename">The filename to get extension from</param>
        /// <returns> The extension (without the .) of the file</returns>
        private static string ExtractExtensionFromFilename(string filename)
        {
            var parts = filename.Split(".");

            return parts[^1];
        }
    }
}
