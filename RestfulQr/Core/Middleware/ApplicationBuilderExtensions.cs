using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using RestfulQr.Core.Auth;
using System;

namespace RestfulQr.Core.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseQrCodeOptionsExtraction(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<QrCodeOptionsExtractionMiddleware>();
        }

        public static AuthenticationBuilder AddApiKeySupport(this AuthenticationBuilder authenticationBuilder, Action<ApiKeyAuthenticationOptions> options)
        {
            return authenticationBuilder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, options);
        }
    }
}
