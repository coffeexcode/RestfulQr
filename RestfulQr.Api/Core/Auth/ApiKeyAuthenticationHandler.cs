using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestfulQr.Api.Core.Providers;
using RestfulQr.Api.Services;
using Serilog;

namespace RestfulQr.Api.Core.Auth
{
public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IApiKeyService apiKeyService;
        private readonly ApiKeyProvider apiKeyProvider;
        private const string ProblemDetailsContentType = "application/problem+json";
        private const string ApiKeyHeaderName = "X-Api-Key";
        public ApiKeyAuthenticationHandler(
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            IApiKeyService apiKeyService,
            ApiKeyProvider apiKeyProvider) : base(options, logger, encoder, clock)
        {
            this.apiKeyService = apiKeyService;
            this.apiKeyProvider = apiKeyProvider;
        }

        /// <summary>
        /// Authenticates a request by ensuring the X-Api-Key header is present with an
        /// API key that exists in a backing store
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
            {
                return AuthenticateResult.NoResult();
            }

            var providedKey = apiKeyHeaderValues.FirstOrDefault();

            Log.Information("Got key: " + providedKey);
            
            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedKey))
            {
                return AuthenticateResult.NoResult();
            }

            var apiKey = await apiKeyService.GetAsync(providedKey);

            if (apiKey == null)
            {
                Log.Warning("An invalid API key was tried during request");
                return AuthenticateResult.Fail("Invalid api key");
            }

            var claims = new List<Claim>
            {
                new Claim("apiKey", apiKey.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
            var identities = new List<ClaimsIdentity> { identity };
            var principal = new ClaimsPrincipal(identities);
            var ticket = new AuthenticationTicket(principal, Options.Scheme);

            // Make the API key accessible via scoped class ApiKeyProvider
            apiKeyProvider.ApiKey = apiKey;

            return AuthenticateResult.Success(ticket);
        }

        /// <summary>
        /// Handle unauthorized calls
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Response.ContentType = ProblemDetailsContentType;
            var problemDetails = AuthorizationProblemDetails.UnauthorizedProblemDetails();

            await Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }

        /// <summary>
        /// Handles a forbidden call
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            Response.ContentType = ProblemDetailsContentType;
            var problemDetails = AuthorizationProblemDetails.ForbiddenProblemDetails();

            await Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}