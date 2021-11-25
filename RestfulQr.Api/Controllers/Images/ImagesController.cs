using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestfulQr.Api.Services.Impl;
using RestfulQr.Core.Util;

namespace RestfulQr.Api.Controllers.Images
{
    [Route("images")]
    [ApiController]
    [AllowAnonymous]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService s3ImageService;

        public ImagesController(IImageService s3ImageService)
        {
            this.s3ImageService = s3ImageService;
        }

        [HttpGet("{locationId}/{filename}")]
        public async Task<ActionResult> GetImage(long locationId, string filename)
        {
            if (!s3ImageService.IsValid(locationId, filename))
            {
                return NotFound();
            }

            var imageBytes = await s3ImageService.GetImageAsync(locationId, filename);

            if (imageBytes == null)
            {
                return NotFound();
            }

            return File(imageBytes, ImageUtils.GetMimeTypeByExtension(filename.Split(".").Last()));
        }
    }
}
