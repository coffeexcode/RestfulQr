using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using RestfulQr.Domain;
using static QRCoder.QRCodeGenerator;

namespace RestfulQr.Api.Core.Middleware
{
/// <summary>
    /// Middleware to extract qr codes from each api request
    /// </summary>
    public class QrCodeRenderOptionsBuilderMiddleware 
    {
        private readonly RequestDelegate next;

        private IConfigurationSection defaults;

        private HttpContext context;

        public QrCodeRenderOptionsBuilderMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration config, QrCodeRenderOptions options)
        {
            defaults = config.GetSection("QrCodeDefaults");
            this.context = context;

            // Overwrite default with any user provided options
            options.EccLevel = GetEccLevel();
            options.LightColour = GetLightColour();
            options.DarkColour = GetDarkColour();
            options.DrawQuietZones = GetDrawQuietZones();
            options.PixelsPerModule = GetPixelsPerModule();
            options.RenderType = GetRenderType();
            options.Preview = GetPreview();

            await next(context);
        }

        private bool GetPreview()
        {
            var hasValue = context.Request.Query.TryGetValue("preview", out var rawPreview);

            if (!hasValue)
            {
                return bool.Parse(defaults["Preview"]);
            }

            var preview = rawPreview.First().ToLower();

            var truthyValues = new string[]
             {
                "t", "true", "y", "yes"
            };

            var falsyValues = new string[]
            {
                "f", "false", "n", "no"
            };

            if (truthyValues.Contains(preview))
            {
                return true;
            }
            else if (falsyValues.Contains(preview))
            {
                return false;
            }

            return bool.Parse(defaults["Preview"]);
        }

        private ECCLevel GetEccLevel()
        {
            var hasValue = context.Request.Query.TryGetValue("eccLevel", out var rawEcc);

            string ecc;

            if (hasValue)
            {
                ecc = rawEcc.First().ToLower();

                if ("lqhm".IndexOf(rawEcc.First().ToLower()) < 0)
                {
                    ecc = defaults["EccLevel"];
                }
            }
            else ecc = defaults["EccLevel"];


            switch (ecc.ToLower())
            {
                case "l":
                    {
                        return ECCLevel.L;
                    }
                case "q":
                    {
                        return ECCLevel.Q;
                    }
                case "h":
                    {
                        return ECCLevel.H;
                    }
                case "m":
                    {
                        return ECCLevel.M;
                    }
                default:
                    {
                        throw new Exception($"Invalid ECC level: {ecc}");
                    }
            };
        }

        private string GetLightColour()
        {
            var hasValue = context.Request.Query.TryGetValue("lightColour", out StringValues rawLightColour);

            string lightColour;

            if (hasValue)
            {
                var regex = new Regex(@"^[0-9A-F]{6}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                lightColour = rawLightColour.First().ToLower();

                if (regex.Match(lightColour).Success)
                {
                    return $"#{rawLightColour.First()}";
                }
            }

            return defaults["LightColour"];

        }

        private string GetDarkColour()
        {
            var hasValue = context.Request.Query.TryGetValue("darkColour", out StringValues rawDarkColour);

            string darkColour;

            if (hasValue)
            {
                var regex = new Regex(@"^[0-9A-F]{6}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                darkColour = rawDarkColour.First().ToLower();

                if (regex.Match(darkColour).Success)
                {
                    return $"#{rawDarkColour.First()}";
                }
            }

            return defaults["DarkColour"];
        }

        private bool GetDrawQuietZones()
        {
            var hasValue = context.Request.Query.TryGetValue("drawQuietZones", out StringValues rawDrawQuietZones);

            if (!hasValue)
            {
                return bool.Parse(defaults["DrawQuietZones"]);
            }

            var truthyValues = new string[]
            {
                "t", "true", "y", "yes"
            };

            var falsyValues = new string[]
            {
                "f", "false", "n", "no"
            };

            if (truthyValues.Contains(rawDrawQuietZones.First()?.ToLower()))
            {
                return true;
            }
            else if (falsyValues.Contains(rawDrawQuietZones.First()?.ToLower()))
            {
                return false;
            }

            return bool.Parse(defaults["DrawQuietZones"]);
        }

        private int GetPixelsPerModule()
        {
            var hasValue = context.Request.Query.TryGetValue("pixelsPerModule", out StringValues rawPixelsPerModule);

            if (!hasValue)
            {
                return int.Parse(defaults["PixelsPerModule"]);
            }

            try
            {
                int amount = int.Parse(rawPixelsPerModule.First());

                if (amount >= 20 && amount <= 40)
                {
                    return amount;
                }

                return int.Parse(defaults["PixelsPerModule"]);
            }
            catch
            {
                return int.Parse(defaults["PixelsPerModule"]);
            }
        }

        private RenderType GetRenderType()
        {
            var hasValue = context.Request.Query.TryGetValue("renderType", out StringValues rawRenderType);

            string renderType;

            if (hasValue)
            {
                renderType = rawRenderType.First().ToLower();
            }
            else renderType = defaults["RenderType"];

            return renderType.ToLower() switch
            {
                "bmp" => RenderType.Bmp,
                "png" => RenderType.Png,
                "svg" => RenderType.Svg,
                "jpeg" or "jpg" => RenderType.Jpeg,
                _ => throw new Exception($"Invalid render type: {renderType}"),
            };
        }
    }
}