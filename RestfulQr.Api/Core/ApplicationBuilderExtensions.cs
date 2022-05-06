using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using RestfulQr.Api.Core.Auth;
using RestfulQr.Api.Core.Middleware;

namespace RestfulQr.Api.Core
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Use middleware to extract the qr code options from each request
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseQrCodeOptionsExtraction(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<QrCodeRenderOptionsBuilderMiddleware>();
        }

        /// <summary>
        /// Configures authentication use use the <see cref="ApiKeyAuthenticationHandler"/> to authorize
        /// requests.
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddApiKeySupport(this AuthenticationBuilder authenticationBuilder, Action<ApiKeyAuthenticationOptions> options)
        {
            return authenticationBuilder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, options);
        }
    }
}