using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RestfulQr.Core.Middleware
{
    public class QrCodeOptionsExtractionMiddleware
    {
        private readonly RequestDelegate next;

        public QrCodeOptionsExtractionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, QrCodeOptions qrCodeOptions)
        {
            await Task.Run(() =>
            {
                SetLightColour(context, qrCodeOptions);
                SetDarkColour(context, qrCodeOptions);
                SetEccLevel(context, qrCodeOptions);
                SetDrawQuietZones(context, qrCodeOptions);
                SetPixelsPerModule(context, qrCodeOptions);
            });

            await next(context);
        }

        private static void SetLightColour(HttpContext context, QrCodeOptions qrCodeOptions)
        {
            if (!context.Request.Query.TryGetValue("light", out var colour) || !ValidateColourCode(colour.First()))
            {
                qrCodeOptions.Light = "#FFFFFF";
            }
            else qrCodeOptions.Light = colour.First();
        }

        private void SetDarkColour(HttpContext context, QrCodeOptions qrCodeOptions)
        {
            if (!context.Request.Query.TryGetValue("dark", out var colour) || !ValidateColourCode(colour.First()))
            {
                qrCodeOptions.Dark = "#000000";
            }
            else qrCodeOptions.Dark = colour.First();
        }

        private static void SetEccLevel(HttpContext context, QrCodeOptions qrCodeOptions)
        {
            if (!context.Request.Query.TryGetValue("ecc", out var ecc))
            {
                qrCodeOptions.ECCLevel = QRCoder.QRCodeGenerator.ECCLevel.L;

                return;
            }

            switch (ecc.First().ToString().ToLower())
            {
                case "l":
                    {
                        qrCodeOptions.ECCLevel = QRCoder.QRCodeGenerator.ECCLevel.L;
                        break;
                    }
                case "m":
                    {
                        qrCodeOptions.ECCLevel = QRCoder.QRCodeGenerator.ECCLevel.M;
                        break;
                    }
                case "q":
                    {
                        qrCodeOptions.ECCLevel = QRCoder.QRCodeGenerator.ECCLevel.Q;
                        break;
                    }
                case "h":
                    {
                        qrCodeOptions.ECCLevel = QRCoder.QRCodeGenerator.ECCLevel.H;
                        break;
                    }
                default:
                    {
                        qrCodeOptions.ECCLevel = QRCoder.QRCodeGenerator.ECCLevel.L;
                        break;
                    }
            }
        }

        private static void SetDrawQuietZones(HttpContext context, QrCodeOptions qrCodeOptions)
        {
            if (!context.Request.Query.TryGetValue("drawQuietZones", out var drawQuietZones))
            {
                qrCodeOptions.DrawQuietZones = false;
            }
            else qrCodeOptions.DrawQuietZones = ParseBoolean(drawQuietZones.First());
        }

        private static void SetPixelsPerModule(HttpContext context, QrCodeOptions qrCodeOptions)
        {
            if (!context.Request.Query.TryGetValue("pixelsPerModule", out var pixelsPerModule))
            {
                qrCodeOptions.PixelsPerModule = 20;
                return;
            }

            try
            {
                var amount = int.Parse(pixelsPerModule.First());

                if (amount < 20 || amount > 40)
                {
                    throw new Exception();
                }

                qrCodeOptions.PixelsPerModule = amount;
            }
            catch
            {
                qrCodeOptions.PixelsPerModule = 40;
            }
        }

        private static bool ValidateColourCode(string colourCode)
        {
            var regex = new Regex(@"^#[0-9A-F]{6}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (regex.Match(colourCode).Success)
            {
                return true;
            }
            else return false;
        }

        private static bool ParseBoolean(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            var truthyValues = new string[]
            {
                "t",
                "true",
                "y",
                "yes"
            };

            if (truthyValues.Contains(input.ToLower()))
            {
                return true;
            }
            else return false;
        }
    }
}
