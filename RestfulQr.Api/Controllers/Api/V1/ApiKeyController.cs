using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestfulQr.Api.Core.Providers;
using RestfulQr.Api.Services;
using RestfulQr.Api.Services.Impl;
using RestfulQr.Domain;
using RestfulQr.Persistence;
using RestfulQr.Persistence.S3;
using Serilog;

namespace RestfulQr.Api.Controllers.Api.V1
{
    [ApiController]
    [Route("api/v1/apiKey")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
    public class ApiKeyController : ControllerBase
    {
        private readonly IApiKeyService apiKeyService;
        private readonly ApiKeyProvider apiKeyProvider;
        private readonly IImageService imageService;

        public ApiKeyController(IApiKeyService apiKeyService, ApiKeyProvider apiKeyProvider, IImageService imageSerivice)
        {
            this.apiKeyService = apiKeyService;
            this.apiKeyProvider = apiKeyProvider;
            this.imageService = imageSerivice;
        }

        /// <summary>
        /// Creates a new api key.
        /// </summary>
        /// <returns>A newly created api key</returns>
        /// <response code="201">Returns the newly created api key</response>
        /// 
        /// <response code="500">The api key could not be created or saved</response>   
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult<ApiKey>> Create()
        {
            try
            {
                var created = await apiKeyService.CreateAsync();

                if (created == null) {
                    return StatusCode(500);
                }

                return Created(string.Empty, created);
            }
            catch (Exception e)
            {
                Log.Error(e, "An error occured while creating a new api key");
                return StatusCode(500);
            }
        }

        [HttpGet("created")]
        [Authorize]
        public async Task<IActionResult> Created()
        {
            try
            {
                var qrCodes = await imageService.GetAllAsync(apiKeyProvider.ApiKey);

                return Ok(qrCodes);
            }
            catch (Exception e)
            {
                Log.Error(e, "An error occured while listing all created qr codes");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Deletes an api key.
        /// </summary>
        /// <response code="200">The operation was successful</response>
        /// <response code="401">User is not authorized to perform the operation</response>
        /// <response code="500">The api key could not be deleted</response>   
        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete()
        {
            try
            {
                if (apiKeyProvider.ApiKey == null)
                {
                    return NoContent();
                }

                var deleted = await apiKeyService.DeleteAsync(apiKeyProvider.ApiKey.Id.ToString());

                await imageService.DeleteAsync(apiKeyProvider.ApiKey);
                
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Error(e, $"An error occured while trying to delete an api key '{apiKeyProvider.ApiKey.ToString}'");
                return StatusCode(500);
            }
        }
    }
}