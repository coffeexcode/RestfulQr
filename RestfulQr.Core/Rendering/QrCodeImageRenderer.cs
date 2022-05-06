using System.IO;
using System.Text;
using System.Threading.Tasks;
using QRCoder;
using RestfulQr.Core.Util;
using RestfulQr.Domain;

namespace RestfulQr.Core.Rendering
{
    internal class QrCodeImageRenderer
    {
        private readonly QrCodeRenderOptions renderOptions;
        public QrCodeImageRenderer(QrCodeRenderOptions renderOptions) 
        {
            this.renderOptions = renderOptions;
        }

        public async Task<byte[]?> RenderAsync(QRCodeData data)
        {
            return await Task.Run(() => {
                byte[]? renderResult = renderOptions.RenderType switch
                {
                    RenderType.Bmp or RenderType.Png or RenderType.Jpeg => RenderImage(data, renderOptions.RenderType),
                    RenderType.Svg => RenderSvg(data),
                    _ => null
                };

                return renderResult;
            });
        }

        private byte[] RenderImage(QRCodeData data, RenderType renderType)
        {
            var qrCode = new QRCode(data);
            var memoryStream = new MemoryStream();
            var imageFormat = ImageUtils.GetImageFormat(renderType);

            qrCode.GetGraphic(renderOptions.PixelsPerModule, renderOptions.DarkColour, renderOptions.LightColour, renderOptions.DrawQuietZones)
                .Save(memoryStream, imageFormat);

            return memoryStream.ToArray();
        }

        private byte[] RenderSvg(QRCodeData data)
        {
            var qrCode = new SvgQRCode(data);

            var render = qrCode.GetGraphic(renderOptions.PixelsPerModule, renderOptions.DarkColour, renderOptions.LightColour, renderOptions.DrawQuietZones);

            return Encoding.ASCII.GetBytes(render);
        }
    }
}